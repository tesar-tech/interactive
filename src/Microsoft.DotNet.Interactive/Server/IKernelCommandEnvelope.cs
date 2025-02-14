﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.DotNet.Interactive.Commands;

namespace Microsoft.DotNet.Interactive.Server
{
    public interface IKernelCommandEnvelope
    {
        KernelCommand Command { get; }

        string CommandType { get; }

        string Token { get; }

        string CommandId { get; }

        Uri OriginUri { get; }

        Uri DestinationUri { get; }
    }
}