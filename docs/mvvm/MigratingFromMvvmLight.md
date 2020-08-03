---
title: Migrating from MvvmLight
author: jamesmcroft
description: This article describes how to migrate MVVM Light solutions to the Windows Community Toolkit Mvvm framework.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, mvvm, mvvmlight, net core, net standard
dev_langs:
  - csharp
---

# Migrating from MvvmLight

This article outlines some of the key differences between the [MVVM Light Toolkit](https://github.com/lbugnion/mvvmlight) and the Windows Community Toolkit MVVM framework to ease your migration. 

## Differences

### ObservableObject
- Namespace (GalaSoft.MvvmLight -> Microsoft.Toolkit.Mvvm.ComponentModel)

#### Changed methods
- RaisePropertyChanged(string) -> OnPropertyChanged(string)
- Set(Expression, ref, value) -> SetProperty(Expression, value)
- Set(propertyName, ref, value) -> SetProperty(ref, value, propertyName)
- Set(ref, value, propertyName) -> SetProperty(ref, value, propertyName)

#### Missing properties
- protected PropertyChangedEventHandler PropertyChangedHandler
  - Can be accessed through PropertyChanged property

#### Missing methods
- VerifyPropertyName
  - No replacement

- RaisePropertyChanged(Expression)
  - No replacement, change from RaisePropertyChanged(() => this.X) to RaisePropertyChanged(nameof(X))

### ViewModelBase
- Type (ViewModelBase -> ObservableRecipient)

#### Changed properties
- MessengerInstance(IMessenger) -> Messenger(IMessenger)

#### Changed methods
- ICleanup.Cleanup -> OnDeactivated
- Set(propertyName, ref, newValue, broadcast) -> SetProperty(ref, newValue, broadcast, propertyName)
  - NewValue is not optional
  - Broadcast is not optional
- Set(ref, newValue, broadcast, propertyName) -> SetProperty(ref, newValue, broadcast, propertyName)
  - NewValue is not optional
  - Broadcast is not optional

#### Broadcast method
- Publishes message with same signature

#### Missing properties
- IsInDesignMode
  - No replacement, used only for design mode in VS or Blend

#### Missing methods
- RaisePropertyChanged(propertyName, oldValue, newValue, broadcast)
  - No direct replacement. 
- RaisePropertyChanged(Expression, oldValue, newValue, broadcast)
  - No direct replacement.
- Set(Expression, ref, newValue, broadcast)
  - No direct replacement.

## Missing

- Android/iOS Xamarin Bindings
