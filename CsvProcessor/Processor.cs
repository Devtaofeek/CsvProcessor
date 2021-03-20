using CsvProcessor.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CsvProcessor
{
	public class Processor
	{
		public async Task<ProductAggregate> Process(Stream csvStream)
		{
			var streamReader = new StreamReader(csvStream);
			List<string> lines = new List<string>();
			while (!streamReader.EndOfStream)
			{
				lines.Add(await streamReader.ReadLineAsync());
			}
			var headers = lines.Take(1).SelectMany(a => a.Split(',')).ToArray();
			var values = lines.Skip(1).ToArray();
			var productTemplateDictionary = new Dictionary<string, int>();
			var productCatalogDictionary = new Dictionary<string, int>();
			var productDetailsDictionary = new Dictionary<string, int>();
			var productCatalogs = new List<ProductCatalog>();
			var productTemplates = new List<ProductTemplate>();
			
			var productCatalogType = typeof(ProductCatalog);
			var productTemplateType = typeof(ProductCatalog);
			for (int i = 0; i < headers.Count(); i++)
			{
				if (productCatalogType.GetProperties().Any(prop => prop.Name == headers[i]))
				{
					productCatalogDictionary.Add(headers[i], i);
				}
				if (productTemplateType.GetProperties().Any(prop => prop.Name == headers[i]))
				{
					productTemplateDictionary.Add(headers[i], i);
				}
				if (productTemplateType.GetProperties().All(prop => prop.Name != headers[i]) && productCatalogType.GetProperties().All(prop => prop.Name != headers[i]))
				{
					productDetailsDictionary.Add(headers[i], i);
				}
			}
			
			var productTemplateGroup = new HashSet<string>();
			for (int i = 0; i < values.Length; i++)
			{
				var extraFields = new Dictionary<string, string>();
				var line = values[i];
				var cells = Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

				if (productTemplateGroup.Contains(cells[productTemplateDictionary["Id"]]))
				{
				  var productCatalog = new ProductCatalog();

					productCatalog.ProductTemplateId = int.Parse(cells[productTemplateDictionary["Id"]]);
					productCatalog.Id = i + 1;
					productCatalog.Name = cells[productCatalogDictionary["Name"]];
					productCatalog.Price = cells[productCatalogDictionary["Price"]];
					productCatalog.Tags = cells[productCatalogDictionary["Tags"]];
					foreach (var item in productDetailsDictionary)
					{
						if (!string.IsNullOrEmpty(cells[productDetailsDictionary[item.Key]]))
						{
							extraFields.Add(item.Key, cells[productDetailsDictionary[item.Key]]);
						}
					}
					productCatalog.Details = JsonSerializer.Serialize(extraFields);
					productCatalogs.Add(productCatalog);
				}
				else
				{
					var productCatalog = new ProductCatalog();

					var productTemplate = new ProductTemplate();

					productTemplateGroup.Add(cells[productTemplateDictionary["Id"]]);
					productTemplate.Id = int.Parse(cells[productTemplateDictionary["Id"]]);
					productTemplate.Name = cells[productTemplateDictionary["Name"]];
					productCatalog.Id = i + 1;
					productCatalog.ProductTemplateId = productTemplate.Id;
					productCatalog.Name = cells[productCatalogDictionary["Name"]];
					productCatalog.Price = cells[productCatalogDictionary["Price"]];
					productCatalog.Tags = cells[productCatalogDictionary["Tags"]];
					foreach (var item in productDetailsDictionary)
					{
						if (!string.IsNullOrEmpty(cells[productDetailsDictionary[item.Key]]))
						{
							extraFields.Add(item.Key, cells[productDetailsDictionary[item.Key]]);
						}
					}
					productTemplates.Add(productTemplate);
					productCatalog.Details = JsonSerializer.Serialize(extraFields);
					productCatalogs.Add(productCatalog);
				}
				
			
				
			  

			}

			var aggregate = new ProductAggregate
			{
				ProductTemplates = productTemplates,
				ProductCatalogs = productCatalogs
			};
			return await Task.FromResult(aggregate);
		}
	}
}