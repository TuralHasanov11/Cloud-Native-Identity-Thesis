﻿namespace PaymentProcessor.IntegrationEvents.Events;

public record OrderPaymentSucceededIntegrationEvent(Guid OrderId) : IntegrationEvent;
