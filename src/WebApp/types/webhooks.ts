export type WebhookSubscription = {
    id: string
    type: string
    date: string
    destinationUrl: string
    token: string
    userId: string
}

export type CreateWebhookSubscriptionRequest = {
    url: string
    token: string
    event: string
    grantUrl: string
}