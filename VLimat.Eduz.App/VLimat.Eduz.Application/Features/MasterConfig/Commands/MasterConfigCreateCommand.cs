using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VLimat.Eduz.Domain.Repositories;
using VLimat.Eduz.Application.Features.MasterConfig.DTOs;



namespace VLimat.Eduz.Application.Features.MasterConfig.Commands
    {
        public record EntityCreateCommand(MasterConfigRequest Request) : IRequest<MasterConfigResponse>;
          public record MasterConfigCreateCommand(MasterConfigRequest Request) : IRequest<MasterConfigResponse>;
}
