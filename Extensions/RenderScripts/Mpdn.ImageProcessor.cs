﻿// This file is a part of MPDN Extensions.
// https://github.com/zachsaw/MPDN_Extensions
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3.0 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library.

using System;
using System.Linq;
using Mpdn.Extensions.Framework.RenderChain;

namespace Mpdn.Extensions.RenderScripts
{
    namespace Mpdn.ImageProcessor
    {
        public class ImageProcessor : RenderChain
        {
            #region Settings

            private string[] m_ShaderFileNames;

            public string[] ShaderFileNames
            {
                get { return m_ShaderFileNames ?? (m_ShaderFileNames = new string[0]); }
                set { m_ShaderFileNames = value; }
            }

            #endregion

            protected override string ShaderPath
            {
                get { return "ImageProcessingShaders"; }
            }

            public string FullShaderPath
            {
                get { return ShaderDataFilePath; }
            }

            public override IFilter CreateFilter(IFilter input)
            {
                return ShaderFileNames.Aggregate(input,
                    (current, filename) => new ShaderFilter(CompileShader(filename), current));
            }
        }

        public class ImageProcessorScript : RenderChainUi<ImageProcessor, ImageProcessorConfigDialog>
        {
            protected override string ConfigFileName
            {
                get { return "Mpdn.ImageProcessor"; }
            }

            public override string Category
            {
                get { return "Processing"; }
            }

            public override ExtensionUiDescriptor Descriptor
            {
                get
                {
                    return new ExtensionUiDescriptor
                    {
                        Guid = new Guid("50CA262F-65B6-4A0F-A8B5-5E25B6A18217"),
                        Name = "Image Processor",
                        Description = GetDescription(),
                    };
                }
            }

            private string GetDescription()
            {
                return Settings.ShaderFileNames.Length == 0
                    ? "Pixel shader pre-/post-processing filter"
                    : string.Join(" ➔ ", Settings.ShaderFileNames);
            }
        }
    }
}
