
using MediatR;
using VLimat.Eduz.Application.Features.MasterConfig.DTOs;
using VLimat.Eduz.Application.Features.MasterConfig.Handlers;

namespace VLimat.Eduz.Application.Features.MasterConfig.Queries
{
    public record GetAllMasterConfigQuery(int AcademicId, string Configuration) : IRequest<IEnumerable<MasterConfigResponse>?>;
    public record EntityGetAllMasterConfigQuery(int AcademicId, string Configuration) : IRequest<IEnumerable<MasterConfigResponse>?>;        
    public record GetMasterConfigQuery(int Id) : IRequest<MasterConfigResponse?>;
    public record EntityGetMasterConfigQuery(int Id) : IRequest<MasterConfigResponse?>;
    
}   