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
    unsafe class VkCommandBuffers : List<VkCommandBuffer>
    {
        public VkCommandBuffers(VkDevice device, in CommandBufferAllocateInfo allocateInfo)
        {
            Vk vk = Vk.GetApi();

            CommandBuffer* commandBuffers = (CommandBuffer*)Mem.AllocArray<CommandBuffer>((int)allocateInfo.CommandBufferCount);

            Result result = vk.AllocateCommandBuffers(device, allocateInfo, commandBuffers);

            if (result != Result.Success)
            {
                ResultException.Throw(result, "Error allocating command buffers");
            }

            for (int i = 0; i < allocateInfo.CommandBufferCount; i++)
            {
                Add(new VkCommandBuffer(commandBuffers[i], device, allocateInfo.CommandPool));
            }

            Mem.FreeArray(commandBuffers);
        }

    }
}
