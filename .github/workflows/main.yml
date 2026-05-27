name: Deploy Backend

on:
  push:
    branches:
      - main

jobs:
  deploy:
    runs-on: self-hosted

    steps:
      - name: Checkout Source
        uses: actions/checkout@v4
        with:
          fetch-depth: 0

      - name: Setup Java
        uses: actions/setup-java@v4
        with:
          distribution: temurin
          java-version: '17'

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'

      - name: Install SonarScanner for .NET
        shell: powershell
        run: dotnet tool install --global dotnet-sonarscanner

      - name: SonarQube Begin
        shell: powershell
        run: |
          dotnet sonarscanner begin `
            /k:"${{ vars.SONAR_PROJECT_KEY }}" `
            /d:sonar.host.url="${{ vars.SONAR_HOST_URL }}" `
            /d:sonar.token="${{ secrets.SONAR_TOKEN }}"

      - name: Restore Backend Project
        shell: powershell
        run: dotnet restore .\VLimat.Eduz.App\VLimat.Eduz.App\VLimat.Eduz.App.csproj

      - name: Build Backend Project
        shell: powershell
        run: dotnet build .\VLimat.Eduz.App\VLimat.Eduz.App\VLimat.Eduz.App.csproj --configuration Release --no-restore

      - name: Run Test Project
        shell: powershell
        run: dotnet test .\VLimat.Eduz.App\VLimat.Eduz.App.Test\VLimat.Eduz.App.Test.csproj --configuration Release

      - name: SonarQube End
        shell: powershell
        run: |
          dotnet sonarscanner end /d:sonar.token="${{ secrets.SONAR_TOKEN }}"

      - name: Stop Existing Container
        shell: powershell
        run: docker stop vlimat-backend
        continue-on-error: true

      - name: Remove Existing Container
        shell: powershell
        run: docker rm vlimat-backend
        continue-on-error: true

      - name: Remove Existing Image
        shell: powershell
        run: docker rmi vlimat-backend
        continue-on-error: true

      - name: Build Docker Image
        shell: powershell
        run: docker build -t vlimat-backend .

      - name: Run Docker Container
        shell: powershell
        run: docker run -d --restart unless-stopped -p 5000:8080 --name vlimat-backend vlimat-backend
