﻿// ==========================================================================
//  AssetEvent.cs
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex Group
//  All rights reserved.
// ==========================================================================

using System;

namespace Squidex.Domain.Apps.Events.Assets
{
    public abstract class AssetEvent : AppEvent
    {
        public Guid AssetId { get; set; }
    }
}
