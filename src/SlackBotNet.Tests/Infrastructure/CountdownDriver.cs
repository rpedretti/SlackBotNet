﻿using SlackBotNet.Drivers;
using SlackBotNet.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SlackBotNet.Tests.Infrastructure
{
    class CountdownDriver : IDriver
    {
        private readonly List<(DateTimeOffset time, string message)> timings
            = new List<(DateTimeOffset, string)>();

        private readonly CountdownEvent countdown;

        public CountdownDriver(CountdownEvent countdown)
        {
            this.countdown = countdown;
        }

        public IReadOnlyList<DateTimeOffset> RecordedTimings
            => this.timings.Select(m => m.time).ToList();

        public IReadOnlyList<string> RecordedMessages
            => this.timings.Select(m => m.message).ToList();

        public Task<SlackBotState> ConnectAsync(string slackToken, IMessageBus bus)
            => Task.FromResult(SlackBotState.Initialize("1", "testbot"));

        public Task DisconnectAsync() => Task.CompletedTask;

        public Task SendMessageAsync(string message)
        {
            this.timings.Add((DateTimeOffset.UtcNow, message));
            this.countdown.Signal();
            return Task.CompletedTask;
        }
    }
}