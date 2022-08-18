# .NET Community Toolkit
This new "MVVM Toolkit" is part of the [.NET Community Toolkit](https://aka.ms/toolkit/dotnet). The .NET Community Toolkit is part of the [.NET Foundation](https://dotnetfoundation.org/).

<a style="text-decoration:none" href="https://aka.ms/mvvmtoolkit/app" target="_blank">
  <img src="https://img.shields.io/badge/Microsoft%20Store-Download-brightgreen" alt="Store link" />
</a>

<a style="text-decoration:none" href="https://nuget.org/packages/CommunityToolkit.Mvvm" target="_blank">
  <img src="https://img.shields.io/nuget/dt/CommunityToolkit.Mvvm" alt="NuGet Downloads" />
</a>

![image](https://user-images.githubusercontent.com/24302614/185499041-1e17df3e-8246-4c4e-a450-c6488dcd23ea.png)

# MVVM Toolkit & Samples

The MVVM library of the .NET Community Toolkit can be found in the [`CommunityToolkit.Mvvm` NuGet package](https://www.nuget.org/packages/CommunityToolkit.Mvvm). It will be known as the "MVVM Toolkit" in short for reference.

The full official documentation can be found [in MS Docs website](https://aka.ms/mvvmtoolkit/docs).

This repo contains initial samples for how to utilize the library as part of our sample app itself. You can download [the sample app in the Microsoft Store here](https://aka.ms/mvvmtoolkit/app).

[![image](https://user-images.githubusercontent.com/24302614/185500042-5240bcc8-3d61-49c6-85c8-b0ab681cdb7a.png)](https://aka.ms/mvvmtoolkit/app)

## Introduction to the MVVM Toolkit

The `CommunityToolkit.Mvvm` package is a modern, fast, and modular MVVM library. It is built around the following principles:

- **Platform and Runtime Independent** - **.NET Standard 2.x** 🚀 (UI Framework Agnostic)
- **Simple to pick-up and use** - No strict requirements on Application structure or coding-paradigms (outside of 'MVVM'ness), i.e., flexible usage.
- **À la carte** - Freedom to choose which components to use.
- **Reference Implementation** - Lean and performant, providing implementations for interfaces that are included in the Base Class Library, but lack concrete types to use them directly.

The package targets .NET Standard so it can be used on any app platform: UWP, WinForms, WPF, Xamarin, Uno, and more; and on any runtime: .NET Native, .NET Core, .NET Framework, or Mono. It runs on all of them. The API surface is identical in all cases. When running on newer runtimes, like .NET 6, it is still optimized to take advantadge of those platforms for the best performance.

[You can read more about the latest features in the release blog here.](https://devblogs.microsoft.com/dotnet/announcing-the-dotnet-community-toolkit-800/)

## Background

This library was inspired by [MVVMLight](https://www.mvvmlight.net/) by Laurent Bugnion. Development was started in April 2020 as a path forward for developers using MVVMLight. We've worked with Laurent, the community, and [Windows Template Studio](https://aka.ms/wts) to ensure successful migration paths for projects using MVVMLight today.

We decided to start from the ground-up as a new project to architect a modern .NET Standard starting point as well as targeting a high-performance implementation which reduces overhead for memory and CPU cycles. Many things in the .NET ecosystem have evolved and changed since the time MVVMLight had begun.

The Windows Community Toolkit seemed like a good home for this new library. This enables it to have broad support from the community, backing from the .NET Foundation, and longevity for the future. It has since spun off into being a major part of the .NET Community Toolkit.

We intend this library to be feature-complete and provide a common basis for app developers to create shared .NET Standard code in their applications for building with the MVVM pattern.

It is not our intent to add or support platform-specific features. We encourage app developers to:

- Use samples to understand how to integrate with their platform
- Build upon this work for simplification of patterns for a specific platform
- If needed, utilize other .NET Foundation supported alternatives like [MVVMCross](https://www.mvvmcross.com/) and [Prism](https://prismlibrary.com/)

## Contributing

If you find an issue with our docs or have suggestions, please check [the latest documentation](https://aka.ms/mvvmtoolkit/docs) and file an issue on the docs repository.

If you'd like to help us with a sample for your own platform, please file an issue here and open up a dialog, or respond to one of the existing open issues tracking known platforms.

We do encourage suggestions, contributions, or platform-agnostic feature requests as well. Please open an issue to start that discussion on the [main repo here](https://github.com/CommunityToolkit/dotnet).

## License

MIT
