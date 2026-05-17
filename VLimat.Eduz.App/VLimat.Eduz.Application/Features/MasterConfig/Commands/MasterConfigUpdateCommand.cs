using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VLimat.Eduz.Domain.Repositories;
using VLimat.Eduz.Application.Features.MasterConfig.DTOs;



namespace VLimat.Eduz.Application.Features.MasterConfig.Commands
    {
        public record EntityUpdateCommand(MasterConfigRequest Request) : IRequest<MasterConfigResponse>;
        public record MasterConfigUpdateCommand(MasterConfigRequest Request) : IRequest<MasterConfigResponse>;
}
