---
title: ObservableValidator
author: Sergio0694
description: A base class for objects implementing the INotifyDataErrorInfo interface.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, mvvm, componentmodel, property changed, notification, errors, validate, validation, binding, net core, net standard
dev_langs:
  - csharp
---

# ObservableValidator

The [`ObservableValidator`](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.componentmodel.ObservableValidator) is a base class implementing the [`INotifyDataErrorInfo`](https://docs.microsoft.com/dotnet/api/system.componentmodel.INotifyDataErrorInfo) interface, providing support for validating properties exposed to other application modules. It also inherits from `ObservableObject`, so it implements [`INotifyPropertyChanged`](https://docs.microsoft.com/dotnet/api/system.componentmodel.inotifypropertychanged) and [`INotifyPropertyChanging`](https://docs.microsoft.com/dotnet/api/system.componentmodel.inotifypropertychanging) as well. It can be used as a starting point for all kinds of objects that need to support both property change notifications and property validation.

> **Platform APIs:** [ObservableValidator](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.componentmodel.ObservableValidator), [ObservableObject](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.mvvm.componentmodel.ObservableObject)

## How it works

`ObservableValidator` has the following main features:

- It provides a base implementation for `INotifyDataErrorInfo`, exposing the `ErrorsChanged` event and the other necessary APIs.
- It provides a series of additional `SetProperty` overloads (on top of the ones provided by the base `ObservableObject` class), that offer the ability of automatically validating properties and raising the necessary events before updating their values.
- It exposes a number of `TrySetProperty` overloads, that are similar to `SetProperty` but with the ability of only updating the target property if the validation is successful, and to return the generated errors (if any) for further inspection.
- It exposes the `ValidateProperty` method, which can be useful to manually trigger the validation of a specific property in case its value has not been updated but its validation is dependent on the value of another property that has instead been updated.
- It exposes the `ValidateAllProperties` method, which automatically executes the validation of all public instance properties in the current instance, provided they have at least one [`[ValidationAttribute]`](https://docs.microsoft.com/dotnet/api/system.componentmodel.dataannotations.validationattribute) applied to them.
- It exposes a `ClearAllErrors` method that can be useful when resetting a model bound to some form that the user might want to fill in again.
- It offers a number of constructors that allow passing different parameters to initialize the [`ValidationContext`](https://docs.microsoft.com/dotnet/api/system.componentmodel.dataannotations.validationcontext) instance that will be used to validate properties. This can be especially useful when using custom validation attributes that might require additional services or options to work correctly.

## Simple property

Here's an example of how to implement a property that supports both change notifications as well as validation:

```csharp
public class RegistrationForm : ObservableValidator
{
    private string name;

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string Name
    {
        get => name;
        set => SetProperty(ref name, value, true);
    }
}
```

Here we are calling the `SetProperty<T>(ref T, T, bool, string)` method exposed by `ObservableValidator`, and that additional `bool` parameter set to `true` indicates that we also want to validate the property when its value is updated. `ObservableValidator` will automatically run the validation on every new value using all the checks that are specified with the attributes applied to the property. Other components (such as UI controls) can then interact with the viewmodel and modify their state to reflect the errors currently present in the viewmodel, by registering to `ErrorsChanged` and using the `GetErrors(string)` method to retrieve the list of errors for each property that has been modified.

## Custom validation methods

Sometimes validating a property requires a viewmodel to have access to additional services, data, or other APIs. There are different ways to add custom validation to a property, depending on the scenario and the level of flexibility that is required. Here is an example of how the [`[CustomValidationAttribute]`](https://docs.microsoft.com/dotnet/api/system.componentmodel.dataannotations.customvalidationattribute) type can be used to indicate that a specific method needs to be invoked to perform additional validation of a property:

```csharp
public class RegistrationForm : ObservableValidator
{
    private readonly IFancyService service;

    public RegistrationForm(IFancyService service)
    {
        this.service = service;
    }

    private string name;

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    [CustomValidation(typeof(RegistrationForm), nameof(ValidateName))]
    public string Name
    {
        get => this.name;
        set => SetProperty(ref this.name, value, true);
    }

    public static ValidationResult ValidateName(string name, ValidationContext context)
    {
        RegistrationForm instance = (RegistrationForm)context.ObjectInstance;
        bool isValid = instance.service.Validate(name);

        if (isValid)
        {
            return ValidationResult.Success;
        }

        return new("The name was not validated by the fancy service");
    }
}
```

In this case we have a static `ValidateName` method that will perform validation on the `Name` property through a service that is injected into our viewmodel. This method receives the name property value and the `ValidationContext` instance in use, which contains things such as the viewmodel instance, the name of the property being validated, and optionally a service provider and some custom flags we can use or set. In this case, we are retrieving the `RegistrationForm` instance from the validation context, and from there we are using the injected service to validate the property. Note that this validation will be executed next to the ones specified in the other attributes, so we are free to combine custom validation methods and existing validation attributes however we like.

## Custom validation attributes

Another way of doing custom validation is by implementing a custom `[ValidationAttribute]`. and then inserting the validation logic into the overridden `IsValid` method. This enables extra flexibility compared to the approach described above, as it makes it very easy to just reuse the same attribute in multiple places.

Suppose we wanted to validate a property based on its relative value with respect to another property in the same viewmodel. The first step would be to define a custom `[GreaterThanAttribute]`, like so:

```csharp
public sealed class GreaterThanAttribute : ValidationAttribute
{
    public GreaterThanAttribute(string propertyName)
    {
        PropertyName = propertyName;
    }

    public string PropertyName { get; }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        object
            instance = validationContext.ObjectInstance,
            otherValue = instance.GetType().GetProperty(PropertyName).GetValue(instance);

        if (((IComparable)value).CompareTo(otherValue) > 0)
        {
            return ValidationResult.Success;
        }

        return new("The current value is smaller than the other one");
    }
}
```

Next we can add this attribute into our viewmodel:

```csharp
public class ComparableModel : ObservableValidator
{
    private int a;

    [Range(10, 100)]
    [GreaterThan(nameof(B))]
    public int A
    {
        get => this.a;
        set => SetProperty(ref this.a, value, true);
    }

    private int b;

    [Range(20, 80)]
    public int B
    {
        get => this.b;
        set
        {
            SetProperty(ref this.b, value, true);
            ValidateProperty(A, nameof(A));
        }
    }
}
```

In this case, we have two numerical properties that must be in a specific range and with a specific relationship between each other (`A` needs to be greater than `B`). We have added the new `[GreaterThanAttribute]` over the first property, and we also added a call to `ValidateProperty` in the setter for `B`, so that `A` is validated again whenever `B` changes (since its validation status depends on it). We just need these two lines of code in our viewmodel to enable this custom validation, and we also get the benefit of having a reusable custom validation attribute that could be useful in other viewmodels in our application as well. This approach also helps with code modularization, as the validation logic is now completely decoupled from the viewmodel definition itself.

## Examples

You can find more examples in the [unit tests](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/UnitTests/UnitTests.Shared/Mvvm).