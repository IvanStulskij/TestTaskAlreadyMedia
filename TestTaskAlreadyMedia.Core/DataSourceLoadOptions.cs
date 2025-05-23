﻿using DevExtreme.AspNet.Data.Helpers;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace TestTaskAlreadyMedia.Core;

[ModelBinder(BinderType = typeof(DataSourceLoadOptionsBinder))]
public class DataSourceLoadOptions : DataSourceLoadOptionsBase
{
}

public class DataSourceLoadOptionsBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var loadOptions = new DataSourceLoadOptions();

        DataSourceLoadOptions.StringToLowerDefault = true;
        DataSourceLoadOptionsParser.Parse(loadOptions, key => bindingContext.ValueProvider.GetValue(key).FirstOrDefault());
        bindingContext.Result = ModelBindingResult.Success(loadOptions);

        return Task.CompletedTask;
    }
}
