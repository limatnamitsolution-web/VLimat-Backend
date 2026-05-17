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
    public interface IProviderMasterConfigFactory
    {
        GetAllMasterConfigQuery CreateGetAllMasterConfig(int academicId, string configuration);
        EntityGetAllMasterConfigQuery CreateEntityGetAllMaster(int academicId, string configuration);

        GetMasterConfigQuery CreateGetMasterConfig(int Id);
        EntityGetMasterConfigQuery CreateEntityGetMasterConfig(int Id);

        MasterConfigCreateCommand CreateCreateMasterConfigCommand(MasterConfigRequest request);
        MasterConfigUpdateCommand  CreateUpdateMasterConfigCommand(MasterConfigRequest request);
    }
}
