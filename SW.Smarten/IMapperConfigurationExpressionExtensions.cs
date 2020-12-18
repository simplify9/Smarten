using System.Globalization;
using AutoMapper;
using System.Linq;
using SW.PrimitiveTypes;
using System.Linq.Expressions;
using System;
using Baseline;

namespace SW.Smarten
{
    static class IMapperConfigurationExpressionExtensions
    {


        public static void CreateMultiLingualMap<TMultiLingualEntity, TTranslation, TDestination>(
            this IMapperConfigurationExpression configuration)
            where TTranslation : class, IEntityTranslation
            where TMultiLingualEntity : IMultiLingualEntity<TTranslation>
        {
            configuration.CreateMap<TTranslation, TDestination>();
            configuration.CreateMap<TMultiLingualEntity, TDestination>().AfterMap((source, destination, context) =>
            {
                //var translation = source.Translations.FirstOrDefault(pt => pt.Language == CultureInfo.CurrentCulture.Name);
                //if (translation != null)
                //{
                //    context.Mapper.Map(translation, destination);
                //    return;
                //}

                //var defaultLanguage = multiLingualMapContext.SettingManager
                //                                            .GetSettingValue(LocalizationSettingNames.DefaultLanguage);
                if (!context.Items.TryGetValue("locale", out var locale))
                {
                    locale = CultureInfo.CurrentCulture.Name;
                }


                var translation = source.Translations.FirstOrDefault(pt => pt.Language.Equals(locale.ToString(), StringComparison.OrdinalIgnoreCase));
                if (translation != null)
                {
                    context.Mapper.Map(translation, destination);
                    return;
                }

                translation = source.Translations.FirstOrDefault();
                if (translation != null)
                {
                    context.Mapper.Map(translation, destination);
                }
            });
        }


    }
}