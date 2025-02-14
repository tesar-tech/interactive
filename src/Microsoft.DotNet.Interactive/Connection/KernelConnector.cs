﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

#nullable enable
namespace Microsoft.DotNet.Interactive.Connection;

public abstract class KernelConnectorBase
{
    public abstract Task<Kernel> ConnectKernelAsync(KernelInfo kernelInfo);
}