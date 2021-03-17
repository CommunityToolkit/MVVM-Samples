# Windows Community Toolkit
This new "MVVM Toolkit" is part of the [Windows Community Toolkit](https://aka.ms/wct). The Windows Community Toolkit is part of the [.NET Foundation](https://dotnetfoundation.org/).

# MVVM Toolkit & Samples

The MVVM library of the Windows Community Toolkit can be found in the [`Microsoft.Toolkit.Mvvm` NuGet package](https://www.nuget.org/packages/Microsoft.Toolkit.Mvvm). It will be known as the "MVVM Toolkit" in short for reference.

The full official documentation can be found [in MS Docs website](https://docs.microsoft.com/en-us/windows/communitytoolkit/mvvm/introduction).

This repo contains initial samples for how to utilize the library.

## Introduction to the MVVM Toolkit

The `Microsoft.Toolkit.Mvvm` package is a modern, fast, and modular MVVM library. It is built around the following principles:

- **Platform and Runtime Independent** - **.NET Standard 2.x** 🚀 (UI Framework Agnostic)
- **Simple to pick-up and use** - No strict requirements on Application structure or coding-paradigms (outside of 'MVVM'ness), i.e., flexible usage.
- **À la carte** - Freedom to choose which components to use.
- **Reference Implementation** - Lean and performant, providing implementations for interfaces that are included in the Base Class Library, but lack concrete types to use them directly.

The package targets .NET Standard so it can be used on any app platform: UWP, WinForms, WPF, Xamarin, Uno, and more; and on any runtime: .NET Native, .NET Core, .NET Framework, or Mono. It runs on all of them. The API surface is identical in all cases.

## Background
This library was inspired by [MVVMLight](https://www.mvvmlight.net/) by Laurent Bugnion. Development was started in April 2020 as a path forward for developers using MVVMLight. We've worked with Laurent, the community, and [Windows Template Studio](https://aka.ms/wts) to ensure successful migration paths for projects using MVVMLight today.

We decided to start from the ground-up as a new project to architect a modern .NET Standard starting point as well as targeting a high-performance implementation which reduces overhead for memory and CPU cycles. Many things in the .NET ecosystem have evolved and changed since the time MVVMLight had begun.

The Windows Community Toolkit seemed like a good home for this new library. This enables it to have broad support from the community, backing from the .NET Foundation, and longevity for the future.

We intend this library to be feature-complete and provide a common basis for app developers to create shared .NET Standard code in their applications for building with the MVVM pattern.

It is not our intent to add or support platform-specific features. We encourage app developers to:

- Use samples to understand how to integrate with their platform
- Build upon this work for simplification of patterns for a specific platform
- If needed, utilize other .NET Foundation supported alternatives like [MVVMCross](https://www.mvvmcross.com/) and [Prism](https://prismlibrary.com/)

## Contributing

If you find an issue with our docs or have suggestions, please file an issue in this repo for now, until our final release, when we will migrate to our main documentation repo.

If you'd like to help us with a sample for your own platform, please file an issue here and open up a dialog, or respond to one of the existing open issues tracking known platforms.

We do encourage suggestions, contributions, or platform-agnostic feature requests as well. Please open an issue to start that discussion on the [main repo here](https://github.com/windows-toolkit/WindowsCommunityToolkit).

## License

MIT
