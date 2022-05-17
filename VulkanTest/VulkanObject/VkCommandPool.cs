﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Core;
using Silk.NET.Core.Native;
using Silk.NET.Maths;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Windowing;
using VulkanTest.Exceptions;
using VulkanTest.Utils;

namespace VulkanTest.VulkanObject
{
    unsafe class VkCommandPool : IDisposable
    {
        readonly Vk vk;
        readonly VkDevice device;

        CommandPool commandPool;
        private bool disposedValue;

        public VkCommandPool(VkDevice device, in CommandPoolCreateInfo info)
        {
            vk = Vk.GetApi();
            this.device = device;

            Result result = vk.CreateCommandPool(device, in info, null, out commandPool);

            if (result != Result.Success)
            {
                ResultException.Throw(result, "Error creating command pool");
            }

        }
                  
        public static implicit operator CommandPool(VkCommandPool c) => c.commandPool;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                vk.DestroyCommandPool(device, commandPool, null);
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~VkCommandPool()
        {
             // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
             Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
