using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Widget;
using ByteSentinelApp.Services;
using System.Linq;
using System.Threading.Tasks;
using Color = Android.Graphics.Color;

namespace ByteSentinelApp.Platforms.Android
{
    [BroadcastReceiver(Label = "Agentes Online Widget", Exported = true)]
    [IntentFilter(new[] { AppWidgetManager.ActionAppwidgetUpdate })]
    [MetaData(AppWidgetManager.MetaDataAppwidgetProvider, Resource = "@xml/agents_widget_info")]
    public class AgentsWidgetProvider : AppWidgetProvider
    {
        public override void OnUpdate(Context? context, AppWidgetManager? appWidgetManager, int[]? appWidgetIds)
        {
            if (context == null || appWidgetManager == null || appWidgetIds == null)
                return;

            foreach (var appWidgetId in appWidgetIds)
            {
                Task.Run(async () =>
                {
                    var api = new ApiService();
                    var agents = await api.GetAgents();

                    var views = new RemoteViews(context.PackageName, Resource.Layout.agents_widget);
                    views.SetTextViewText(Resource.Id.appTitle, "ByteSentinel");

                    var agentsSample = agents.Take(2).ToList();

                    for (int i = 0; i < agentsSample.Count; i++)
                    {
                        var agent = agentsSample[i];
                        var containers = await api.GetContainers(agent.AgentId);

                        int running = containers.Count(c => c.Status.Contains("Up", System.StringComparison.OrdinalIgnoreCase));
                        int stopped = containers.Count(c => !c.Status.Contains("Up", System.StringComparison.OrdinalIgnoreCase));

                        int titleId = context.Resources.GetIdentifier($"agentTitle{i + 1}", "id", context.PackageName);
                        int runningId = context.Resources.GetIdentifier($"runningCount{i + 1}", "id", context.PackageName);
                        int stoppedId = context.Resources.GetIdentifier($"stoppedCount{i + 1}", "id", context.PackageName);

                        views.SetTextViewText(titleId, $"Agente {agent.AgentId}");
                        views.SetTextViewText(runningId, running.ToString());
                        views.SetTextColor(runningId, Color.Green);
                        views.SetTextViewText(stoppedId, stopped.ToString());
                        views.SetTextColor(stoppedId, Color.Red);
                    }

                    // clique abre o app em qualquer parte
                    var openAppIntent = new Intent(context, typeof(MainActivity));
                    openAppIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTop);

                    var pendingIntent = PendingIntent.GetActivity(
                        context,
                        0,
                        openAppIntent,
                        PendingIntentFlags.UpdateCurrent | (Build.VERSION.SdkInt >= BuildVersionCodes.S ? PendingIntentFlags.Immutable : 0)
                    );

                    views.SetOnClickPendingIntent(Resource.Id.widgetRoot, pendingIntent);

                    appWidgetManager.UpdateAppWidget(appWidgetId, views);
                });
            }
        }
    }
}
