namespace Sitecore.Support.XA.Feature.Composites.EventHandlers
{
  using Sitecore.Data.Items;
  using Sitecore.Pipelines;
  using Sitecore.Pipelines.ResolveRenderingDatasource;
  using Sitecore.XA.Foundation.Presentation.Layout;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text.RegularExpressions;

  public class SaveCompositeItems : Sitecore.XA.Feature.Composites.EventHandlers.SaveCompositeItems
  {
    protected override Item GetCompositeDatasource(Item contextItem, string datasource)
    {
      ResolveRenderingDatasourceArgs resolveRenderingDatasourceArgs = new ResolveRenderingDatasourceArgs(datasource);
      resolveRenderingDatasourceArgs.CustomData["contextItem"] = contextItem;
      CorePipeline.Run("resolveRenderingDatasource", resolveRenderingDatasourceArgs, false);
      return contextItem.Database.GetItem(resolveRenderingDatasourceArgs.Datasource, contextItem.Language);
    }

    protected override void TransformPlaceholderPaths(RenderingModel compositeRendering, int index, IEnumerable<RenderingModel> renderings)
    {
      ++index;
      string dynamicPlaceholderId = this.GetDynamicPlaceholderId(compositeRendering);
      string pattern = string.Format("(?<={0}+)-{1}-{2}", (object)"section-[title|content]", (object)index, (object)dynamicPlaceholderId);
      foreach (RenderingModel rendering in renderings)
      {
        rendering.Placeholder = GetCompositeSectionPlaceholder(new Placeholder(rendering.Placeholder));
        rendering.Placeholder = Regex.Replace(rendering.Placeholder, pattern, string.Empty);
      }
    }

    public static string GetCompositeSectionPlaceholder(Placeholder ph)
    {
      Placeholder placeholder = new Placeholder(ph);
      string compositeSectionPlaceholderName = GetCompositeSectionPlaceholderName(placeholder);
      if (compositeSectionPlaceholderName == null)
      {
        return null;
      }
      int num = placeholder.Parts.LastIndexOf(compositeSectionPlaceholderName);
      for (int i = 0; i < num; i++)
      {
        placeholder.Parts.RemoveAt(0);
      }
      return placeholder.GetPlaceholderPath();
    }

    public static string GetCompositeSectionPlaceholderName(Placeholder ph) =>
           ph.Parts.LastOrDefault(p => Regex.Match(p, $"{"section-[title|content]"}+-\\d+-\\d").Success);
  }
}