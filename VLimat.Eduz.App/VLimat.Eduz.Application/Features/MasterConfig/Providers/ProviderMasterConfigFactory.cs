using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLimat.Eduz.Application.Features.MasterConfig.Commands;
using VLimat.Eduz.Application.Features.MasterConfig.DTOs;
using VLimat.Eduz.Application.Features.MasterConfig.Queries;

namespace VLimat.Eduz.Application.Features.MasterConfig.Providers
{
    public class ProviderMasterConfigFactory : IProviderMasterConfigFactory
    {
        public GetAllMasterConfigQuery CreateGetAllMasterConfig(int academicId, string configuration)
            => new GetAllMasterConfigQuery(academicId, configuration);

        public EntityGetAllMasterConfigQuery CreateEntityGetAllMaster(int academicId, string configuration)
            => new EntityGetAllMasterConfigQuery(academicId, configuration);

        public GetMasterConfigQuery CreateGetMasterConfig(int Id)
            => new GetMasterConfigQuery(Id);

        public EntityGetMasterConfigQuery CreateEntityGetMasterConfig(int Id)
            => new EntityGetMasterConfigQuery(Id);

        public MasterConfigCreateCommand CreateCreateMasterConfigCommand(MasterConfigRequest request)
            => new MasterConfigCreateCommand(request);


        public MasterConfigUpdateCommand CreateUpdateMasterConfigCommand(MasterConfigRequest request)
            => new MasterConfigUpdateCommand(request);

    }
}
