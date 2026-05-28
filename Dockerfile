- name: Build Docker Image
  shell: powershell
  run: |
    $env:DOCKER_BUILDKIT="0"
    docker build -t vlimat-backend:new .

- name: Stop Existing Container
  run: docker stop vlimat-backend
  continue-on-error: true

- name: Remove Existing Container
  run: docker rm vlimat-backend
  continue-on-error: true

- name: Remove Existing Image
  run: docker rmi vlimat-backend
  continue-on-error: true

- name: Tag New Image
  run: docker tag vlimat-backend:new vlimat-backend

- name: Run Docker Container
  run: docker run -d --restart unless-stopped -p 5000:8080 --name vlimat-backend vlimat-backend
