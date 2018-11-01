namespace Sitecore.Support.XA.Feature.Composites.EventHandlers
{
  using Sitecore.Data.Items;
  using Sitecore.Pipelines;
  using Sitecore.Pipelines.ResolveRenderingDatasource;

  public class SaveCompositeItems : Sitecore.XA.Feature.Composites.EventHandlers.SaveCompositeItems
  {
    protected override Item GetCompositeDatasource(Item contextItem, string datasource)
    {
      ResolveRenderingDatasourceArgs resolveRenderingDatasourceArgs = new ResolveRenderingDatasourceArgs(datasource);
      resolveRenderingDatasourceArgs.CustomData["contextItem"] = contextItem;
      CorePipeline.Run("resolveRenderingDatasource", resolveRenderingDatasourceArgs, false);
      return contextItem.Database.GetItem(resolveRenderingDatasourceArgs.Datasource);
    }
  }
}