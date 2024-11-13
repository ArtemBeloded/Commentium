using MassTransit;
using Microsoft.Extensions.Logging;

namespace Commentium.Application.Comments.Create
{
    public sealed class CommentCreatedEventConsumer : IConsumer<CommentCreatedEvent>
    {
        private readonly ILogger<CommentCreatedEventConsumer> _logger;

        public CommentCreatedEventConsumer(ILogger<CommentCreatedEventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<CommentCreatedEvent> context)
        {
            _logger.LogInformation("Comment created: {@Comment}", context.Message);

            return Task.CompletedTask;
        }
    }
}
