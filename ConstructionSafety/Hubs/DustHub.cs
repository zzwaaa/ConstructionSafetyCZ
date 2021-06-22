using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionSafety.Hubs
{
    public class DustHub:Hub
    {
    //    private readonly IList<string> UserList = UserInfo.userList;
    //    private readonly static Dictionary<string, string> _connections = new Dictionary<string, string>();
    //    private readonly IHubContext<DustHub> _context;
    //    private readonly InterfaceSysDBContext db;
    //    public DustHub(IHubContext<DustHub> context, InterfaceSysDBContext db)
    //    {
    //        _context = context;
    //        this.db = db;
    //    }
    //    public async Task SendMessage(string user, string message)
    //    {
    //        await _context.Clients.Group("user").SendAsync("ReceiveMessage", user, message);
    //    }

    //    /// <summary>
    //    /// 发送实时数据
    //    /// </summary>
    //    /// <param name="belongedTo"></param>
    //    /// <param name="recordNumber"></param>
    //    /// <param name="history"></param>
    //    /// <returns></returns>
    //    public async Task SendExceedDust(string projectInfoId, DustHistory history)
    //    {
    //        try
    //        {
    //            await _context.Clients.Group(projectInfoId).SendAsync("UpDateExceedDust", history);
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.Error("发送实时扬尘数据：" + ex.Message);
    //        }

    //    }

    //    /// <summary>
    //    /// 登陆，主动连接
    //    /// </summary>
    //    /// <param name="name"></param>
    //    /// <returns></returns>
    //    public async Task SendLogin(string projectInfoId)
    //    {
    //        await Groups.AddToGroupAsync(Context.ConnectionId, projectInfoId);
    //        _connections.Add(Context.ConnectionId, projectInfoId);
    //        try
    //        {
    //            var history = await db.DustRealTimes
    //                .Where(w => w.ProjectInfoId == projectInfoId && w.DeleteMark == 0)
    //                .OrderByDescending(o => o.Id)
    //                .AsNoTracking()
    //                .FirstOrDefaultAsync();
    //            await Clients.Caller.SendAsync("UpDateExceedDust", history);
    //        }
    //        catch (Exception ex)
    //        {

    //        }
    //    }

    //    public override Task OnConnectedAsync()
    //    {
    //        Clients.Caller.SendAsync("SendLogin");
    //        return base.OnConnectedAsync();
    //    }

    //    public override async Task OnDisconnectedAsync(Exception exception)
    //    {
    //        //掉线移除用户

    //        if (_connections.ContainsKey(Context.ConnectionId))
    //        {
    //            await Groups.RemoveFromGroupAsync(Context.ConnectionId, _connections[Context.ConnectionId]);
    //        }

    //        await base.OnDisconnectedAsync(exception);
    //    }
    //}

    //public class UserInfo
    //{
    //    public static IList<string> userList = new List<string>();
    //}
}
}
