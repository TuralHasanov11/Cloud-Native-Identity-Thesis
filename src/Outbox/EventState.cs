namespace Outbox;

public enum EventState
{
    NotPublished = 0,
    InProgress = 1,
    Published = 2,
    PublishedFailed = 3,
    Processed = 4,
    ProcessedFailed = 5,
}
