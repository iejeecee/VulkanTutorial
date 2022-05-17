﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    unsafe class VkPipelineLayout : IDisposable
    {
        PipelineLayout pipelineLayout;
        readonly Vk vk;
        readonly VkDevice device;
        private bool disposedValue;

        public VkPipelineLayout(VkDevice device, in PipelineLayoutCreateInfo createInfo)
        {
            vk = Vk.GetApi();
            this.device = device;

            Result result = vk.CreatePipelineLayout(device, createInfo, null, out pipelineLayout);

            if (result != Result.Success)
            {
                ResultException.Throw(result, "Error creating pipeline layout");
            }
        }
       
        public static implicit operator PipelineLayout(VkPipelineLayout p) => p.pipelineLayout;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                vk.DestroyPipelineLayout(device, this, null);

                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~VkPipelineLayout()
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



