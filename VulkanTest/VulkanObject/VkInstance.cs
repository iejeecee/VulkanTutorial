﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Silk.NET.Core;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;

namespace VulkanTest.VulkanObject
{
    unsafe class VkInstance : IDisposable
    {
        readonly Vk vk;
        Instance instance;

        readonly string[] validationLayers;
        private bool disposedValue;

        public VkInstance(
            string appName,
            string engineName,
            Version32 apiVersion,
            string[] extensions = null,
            string[] layers = null,
            DebugUtilsMessengerCreateInfoEXT? messengerInfo = null)
        {
            if (extensions == null) extensions = Array.Empty<string>();

            vk = Vk.GetApi();

            if (layers != null)
            {
                validationLayers = CheckAvailableValidationLayers(layers);
                if (validationLayers is null)
                {
                    throw new NotSupportedException("Validation layers requested, but not available!");
                }
            }

            ApplicationInfo appInfo = new
            (
                pApplicationName: (byte*)Marshal.StringToHGlobalAnsi(appName),
                applicationVersion: new Version32(1, 0, 0),
                pEngineName: (byte*)Marshal.StringToHGlobalAnsi(engineName),
                engineVersion: new Version32(1, 0, 0),
                apiVersion: apiVersion
            );

            InstanceCreateInfo createInfo = new
            (
                pApplicationInfo: &appInfo
            );

            createInfo.EnabledExtensionCount = (uint)extensions.Length;
            createInfo.PpEnabledExtensionNames = (byte**)SilkMarshal.StringArrayToPtr(extensions);

            if (validationLayers != null)
            {
                createInfo.EnabledLayerCount = (uint)validationLayers.Length;
                createInfo.PpEnabledLayerNames = (byte**)SilkMarshal.StringArrayToPtr(validationLayers);
            }

            if (messengerInfo.HasValue)
            {
                DebugUtilsMessengerCreateInfoEXT info = messengerInfo.Value;
                createInfo.PNext = &info;
            }

            Result result = vk.CreateInstance(in createInfo, null, out instance);
            if (result != Result.Success)
            {
                throw new Exception("Failed to create instance!");
            }

            SilkMarshal.Free((nint)createInfo.PpEnabledLayerNames);
            SilkMarshal.Free((nint)createInfo.PpEnabledExtensionNames);

            Marshal.FreeHGlobal((nint)appInfo.PApplicationName);
            Marshal.FreeHGlobal((nint)appInfo.PEngineName);

        }

        string[] CheckAvailableValidationLayers(string[] layers)
        {
            uint nrLayers = 0;
            vk.EnumerateInstanceLayerProperties(&nrLayers, null);

            LayerProperties[] availableLayers = new LayerProperties[nrLayers];

            vk.EnumerateInstanceLayerProperties(&nrLayers, availableLayers);

            var availableLayerNames = availableLayers.Select(availableLayer => Marshal.PtrToStringAnsi((nint)availableLayer.LayerName)).ToArray();

            if (layers.All(validationLayerName => availableLayerNames.Contains(validationLayerName)))
            {
                return layers;
            }

            return null;
        }

        public VkPhysicalDevice[] EnumerateDevices()
        {
            uint deviceCount = 0;
            vk.EnumeratePhysicalDevices(instance, &deviceCount, null);

            if (deviceCount == 0)
            {
                throw new Exception("failed to find GPUs with Vulkan support!");
            }

            PhysicalDevice[] devices = new PhysicalDevice[deviceCount];

            vk.EnumeratePhysicalDevices(instance, &deviceCount, devices);

            VkPhysicalDevice[] vkPhysicalDevices = new VkPhysicalDevice[deviceCount];

            for (int i = 0; i < deviceCount; i++)
            {
                vkPhysicalDevices[i] = new VkPhysicalDevice(devices[i]);
            }

            return vkPhysicalDevices;
        }

        public static implicit operator Instance(VkInstance i) => i.instance;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                vk.DestroyInstance(instance, null);

                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~VkInstance()
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
