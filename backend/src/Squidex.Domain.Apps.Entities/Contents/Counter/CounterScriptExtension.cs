﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System;
using System.Threading.Tasks;
using Orleans;
using Squidex.Domain.Apps.Core.Scripting;
using Squidex.Infrastructure;

namespace Squidex.Domain.Apps.Entities.Contents.Counter
{
    public sealed class CounterScriptExtension : IScriptExtension
    {
        private readonly IGrainFactory grainFactory;

        public CounterScriptExtension(IGrainFactory grainFactory)
        {
            Guard.NotNull(grainFactory, nameof(grainFactory));

            this.grainFactory = grainFactory;
        }

        public void Extend(ExecutionContext context, bool async)
        {
            if (context.TryGetValue("appId", out var temp) && temp is DomainId appId)
            {
                var engine = context.Engine;

                engine.SetValue("incrementCounter", new Func<string, long>(name =>
                {
                    return Increment(appId, name);
                }));

                engine.SetValue("resetCounter", new Func<string, long, long>((name, value) =>
                {
                    return Reset(appId, name, value);
                }));
            }
        }

        private long Increment(DomainId appId, string name)
        {
            var grain = grainFactory.GetGrain<ICounterGrain>(appId.ToString());

            return Task.Run(() => grain.IncrementAsync(name)).Result;
        }

        private long Reset(DomainId appId, string name, long value)
        {
            var grain = grainFactory.GetGrain<ICounterGrain>(appId.ToString());

            return Task.Run(() => grain.ResetAsync(name, value)).Result;
        }
    }
}
