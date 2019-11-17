﻿// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Api.cs" company="Kaplas">
// © Kaplas. Licensed under MIT. See LICENSE for details.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace ParLib
{
    using System.Text;

    /// <summary>
    /// Exposes the public functionality of the library.
    /// </summary>
    public static partial class Api
    {
        static Api()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}
