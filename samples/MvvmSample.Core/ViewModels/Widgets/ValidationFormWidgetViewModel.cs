// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmSample.Core.Services;

namespace MvvmSample.Core.ViewModels.Widgets;

/// <summary>
/// A viewmodel for the validation widget.
/// </summary>
public partial class ValidationFormWidgetViewModel : ObservableValidator
{
    private readonly IDialogService DialogService;

    public ValidationFormWidgetViewModel(IDialogService dialogService)
    {
        DialogService = dialogService;
    }

    public event EventHandler? FormSubmissionCompleted;
    public event EventHandler? FormSubmissionFailed;

    [ObservableProperty]
    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    private string? firstName;

    [ObservableProperty]
    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    private string? lastName;

    [ObservableProperty]
    [Required]
    [EmailAddress]
    private string? email;

    [ObservableProperty]
    [Required]
    [Phone]
    private string? phoneNumber;

    [RelayCommand]
    private void Submit()
    {
        ValidateAllProperties();

        if (HasErrors)
        {
            FormSubmissionFailed?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            FormSubmissionCompleted?.Invoke(this, EventArgs.Empty);
        }
    }

    [RelayCommand]
    private void ShowErrors()
    {
        string message = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));

        _ = DialogService.ShowMessageDialogAsync("Validation errors", message);
    }
}
