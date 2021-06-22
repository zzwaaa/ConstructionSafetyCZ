using Common.Common;
using ConstructionSafety.Controllers.Systems;
using MCUtil.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.ViewModels;
using WisdomDbCore.WisdomModels;
//using WisdomDbCore.WisdomModels;

namespace ConstructionSafety.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PersonInfoController : ControllerBase
    {
        private readonly ILogger<PersonInfoController> _logger;
        private readonly WisdomPlatDBContext _context;
        private readonly ProjLiefInsDBContext _life;
        private OssFileSetting _ossFileSetting;

        public PersonInfoController(IOptions<OssFileSetting> oss, ILogger<PersonInfoController> logger, WisdomPlatDBContext context,ProjLiefInsDBContext projLief) 
        {
            _logger = logger;
            _context = context;
            _life = projLief;
            _ossFileSetting = oss.Value;
        }


        /// <summary>
        /// 查询人员的基本信息以及积分排名 项目积分排名
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public async Task<ResponseViewModel<object>> GetPersonInfo(string personId)
        {
            try
            {

                var projInfos = await _life.ProjectPoints
                               .Where(s => s.PersonId == personId).FirstOrDefaultAsync();
                if (projInfos==null)
                {
                    return ResponseViewModel<object>.Create(Status.ERROR, Message.ERROR, "您当前未绑定项目，暂无信息");
                }
                var IntgralInfo = _life.IntegralInfos
                               .Where(p => p.PersonId == projInfos.PersonId)
                               .FirstOrDefault();
                //var allRanks = _life.IntegralInfos.OrderByDescending(s => s.IntegralNums).ToList();
                var personNums = _life.IntegralInfos.Where(w => w.PersonId == personId).Select(s => s.IntegralNums).FirstOrDefault();
                var personRank = _life.IntegralInfos.Where(w => w.IntegralNums > personNums).Count()+1;
                var proRank = (from a in _life.IntegralInfos
                               group a by new
                               {
                                   a.ProjectInfoId
                               } into g
                               select new
                               {
                                   g.Key.ProjectInfoId,
                                   count = (int?)g.Sum(p => p.IntegralNums)
                               }).OrderByDescending(o => o.count).ToList();
                var projNums = proRank.Where(s => s.ProjectInfoId == projInfos.ProjectInfoId).Select(s => s.count).FirstOrDefault();
                var projRanks = proRank.Where(s => s.count > projNums).Count() + 1;
                //var projRank = 0;
                //foreach (var item in proRank)
                //{
                //    if (item.ProjectInfoId == projInfos.ProjectInfoId)
                //    {
                //        projRank = proRank.IndexOf(item) + 1;
                //    }
                //}
                //int perRank = 0;
                //foreach (var item in allRanks) 
                //{
                //    if (item.PersonId == IntgralInfo.PersonId)
                //    {
                //        perRank = allRanks.IndexOf(item) + 1;
                //    }
                //}

                ProjectPoint points = new ProjectPoint
                {
                    PersonId = IntgralInfo.PersonId,
                    PersonName = projInfos.PersonName,
                    ProjectInfoId = projInfos.ProjectInfoId,
                    ProjectName = projInfos.ProjectName,
                    AllRanks = personRank.ToString(),
                    ProjectRanks = projRanks.ToString()
                };
                return ResponseViewModel<object>.Create(Status.SUCCESS, Message.SUCCESS, points);
            }
            catch (Exception ex)
            {
                _logger.LogError("获取信息：" + ex.Message, ex);
                return ResponseViewModel<object>.Create(Status.ERROR, Message.ERROR);
            }
        }

        ///// <summary>
        ///// 根据手机号添加人员信息 以及初始化积分为0
        ///// </summary>
        ///// <param name="phone"></param>
        ///// <returns></returns>
        //public async Task<ResponseViewModel<string>> GetProjInfo(string phone)
        //{
        //    try
        //    {
        //        var userInfo = _context.UserInfos
        //                     .Where(s => s.Deleted == 0 && s.Status == 1 && s.Telephone == phone).FirstOrDefault();
        //        var projInfos = await _context.ProjectInfo
        //                       .Where(p => p.ProjectInfoId == userInfo.ProjectInfoId).ToListAsync();
        //        var projName = projInfos.Select(p => p.ProjectName).FirstOrDefault();
        //        var perId = SecurityManage.GuidUpper();
        //        var projectIds = projInfos.Select(s => s.ProjectInfoId).FirstOrDefault();
        //        ProjectPoint project = new ProjectPoint
        //        {
        //            ProjectInfoId = projectIds,
        //            ProjectName = projName,
        //            PersonId = perId,
        //            PersonName = userInfo.Name,
        //            AllRanks = "0",
        //            ProjectRanks="0"
        //        };
        //        _life.ProjectPoints.Add(project);
        //        IntegralInfo info = new IntegralInfo
        //        {
        //            IntegralNums = 0,
        //            PersonId = project.PersonId,
        //            ProjectInfoId = projectIds,
        //            UnIntegralNums = 0
        //        };
        //        _life.IntegralInfos.Add(info);
        //        await _life.SaveChangesAsync();
        //        return ResponseViewModel<string>.Create(Status.SUCCESS,Message.SUCCESS);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("添加信息：" + ex.Message, ex);
        //        return ResponseViewModel<string>.Create(Status.ERROR, Message.ERROR);
        //    }
        //}

        /// <summary>
        /// 根据人员ID获取所获取积分的记录
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public async Task<ResponseViewModel<object>> GetIntegralInfo(string personId) 
        {
            if (string.IsNullOrEmpty(personId))
            {
                return ResponseViewModel<object>.Create(Status.FAIL, Message.FAIL);
            }
            try
            {
                var integralInfo = await _life.IntegralInfos.Where(s => s.PersonId == personId)
                                  .FirstOrDefaultAsync();
                if (integralInfo==null)
                {
                    return ResponseViewModel<object>.Create(Status.ERROR, Message.ERROR, "您当前未绑定项目，暂无信息");
                }
                var integralType = await _life.IntegralRecords.Where(s => s.PersonId == personId).Select(s => s.IntegralType).Distinct().ToListAsync();
                var sum = await _life.IntegralRecords.Where(s => s.PersonId == personId).CountAsync();


                var lookInfo = await _life.IntegralRecords.Where(s => integralType.Contains(s.IntegralType) && s.PersonId == personId)
                               .ToListAsync();

                var result = new IntegralInfoNumViewModel
                {
                    IntegralNums = integralInfo.IntegralNums,
                    UnIntegralNums = integralInfo.UnIntegralNums,
                    integralCounts = new IntegralCount
                    {
                        NoticeCount = lookInfo.Where(s => s.IntegralType == "看通知").Select(s => s.IntegralNums).Sum(),
                        ImgTxtCount = lookInfo.Where(s => s.IntegralType == "看图文").Select(s => s.IntegralNums).Sum(),
                        VideoCount = lookInfo.Where(s => s.IntegralType == "看视频").Select(s => s.IntegralNums).Sum(),
                        GameCount = lookInfo.Where(s => s.IntegralType == "玩游戏").Select(s => s.IntegralNums).Sum()
                    }

                };

                return ResponseViewModel<object>.Create(Status.SUCCESS, Message.SUCCESS, result);             
            }
            catch (Exception ex)
            {
                _logger.LogError("百分比：" + ex.Message, ex);
                return ResponseViewModel<object>.Create(Status.ERROR, Message.ERROR);
            }
        }

        /// <summary>
        /// 上传通知
        /// </summary>
        /// <param name="iform"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseViewModel<string>> UploadWord([FromForm] IFormCollection iform) 
        {
            try
            {
                var file = iform.Files[0];
                var url = Util.UploadAnyFile(_ossFileSetting, file, "NoticeWord");
                return ResponseViewModel<string>.Create(Status.SUCCESS,Message.SUCCESS,url);
            }
            catch (Exception ex)
            {
                _logger.LogError("上传通知：" + ex.Message, ex);
                return ResponseViewModel<string>.Create(Status.ERROR, Message.ERROR);
            }
        }


        /// <summary>
        /// 获取积分
        /// </summary>
        /// <param name="InteType"></param>
        /// <param name="IntNums"></param>
        /// <param name="personId"></param>
        /// <param name="projectInfoId"></param>
        /// <returns></returns>
        public async Task<ResponseViewModel<string>> GetPointInfos(int InteType,int IntNums, string personId, string projectInfoId,string noticeId)
        {
            try
            {
                string IntegralType = null;
                switch (InteType)
                {
                    case 0: IntegralType = "看通知"; break;
                    case 1: IntegralType = "看图文"; break;
                    case 2: IntegralType = "看视频"; break;
                    case 3: IntegralType = "玩游戏"; break;
                    default:
                        break;
                }
                var now = DateTime.Now;
                IntegralRecord integral = new IntegralRecord
                {
                    IntegralNums = IntNums,
                    IntegralType = IntegralType,
                    PersonId = personId,
                    ProjectInfoId = projectInfoId,
                    NoticeId=noticeId,  
                    CreateTime = now
                };
                _life.IntegralRecords.Add(integral);
                await _life.SaveChangesAsync();
                var intNums = await _life.IntegralRecords.Where(s => s.PersonId == personId && s.ProjectInfoId == projectInfoId).Select(s => s.IntegralNums).SumAsync();
                var info = await _life.IntegralInfos.Where(s => s.PersonId == personId && s.ProjectInfoId == projectInfoId).FirstOrDefaultAsync();
                info.IntegralNums = intNums;
                info.CreateTime = now;
                _life.IntegralInfos.UpdateRange(info);
                if (InteType==2)
                {
                    var news = _life.PersonNews.Where(s => s.NoticeId == noticeId && s.PersonId == personId).FirstOrDefault();
                    if (news == null)
                    {
                        PersonNew person = new PersonNew
                        {
                            MID = "1",
                            ProjectInfoId = projectInfoId,
                            NoticeId = noticeId,
                            PersonId = personId,
                            CreateTime=now
                        };
                        _life.PersonNews.Add(person);
                        await _life.SaveChangesAsync();
                    }
                }
                await _life.SaveChangesAsync();
                return ResponseViewModel<string>.Create(Status.SUCCESS,Message.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError("上传通知：" + ex.Message, ex);
                return ResponseViewModel<string>.Create(Status.ERROR, Message.ERROR);
            }
        }

        /// <summary>
        /// 获取通知、图文、视频信息列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="projectInfoIds"></param>
        /// <returns></returns>
        public async Task<ResponseViewModel<object>> GetNoticeLists(int type,int pageIndex,int pageSize,string projectInfoIds,string personId) 
        {
            try
            {
                string noticeType = null;
                switch (type)
                {
                    case 0: noticeType = "通知"; break;
                    case 1: noticeType = "图文"; break;
                    case 2: noticeType = "视频"; break;
                    default:
                        break;
                }
                var notids = await _life.FileUploads.Where(s => s.DeleteMark == 0 && projectInfoIds==s.ProjectInfoId).ToListAsync();
                var news =  _life.PersonNews.Where(s => notids.Select(s => s.NoticeId).Contains(s.NoticeId) && s.MID == "1" && s.PersonId == personId && projectInfoIds==s.ProjectInfoId).FirstOrDefault();

                //var mid = _life.PersonNews.Where(w => notids.Select(s => s.NoticeId).Contains(w.NoticeId) && w.PersonId == personId).Select(s => s.MID).FirstOrDefault() ?? "0";
                //var unIntgal = _life.IntegralRecords.Where(w => w.PersonId == personId && w.ProjectInfoId == projectInfoIds && notids.Select(s => s.NoticeId).Contains(w.NoticeId)).Select(s => s.NoticeId).Any();

                var fileList = await _life.FileUploads
                .Where(f => projectInfoIds==f.ProjectInfoId && f.NoticeType == noticeType && f.DeleteMark == 0 )//&& notids.Select(s => s.NoticeId).Contains(f.NoticeId))
                .Select(s => new FileUploadViewModel
                {
                    NoticeId = s.NoticeId,
                    ImgUuId = s.ImgUuId,
                    Title = s.Title,
                    ProjectInfoId = s.ProjectInfoId,
                    CreateDate = s.CreateDate,
                    Content = s.Content,
                    NoticeType=s.NoticeType
                    //MID=mid,
                    //Integal=unIntgal
                }).OrderBy(o=>o.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .ToListAsync();
                var imgs = await _life.FileImgs.Where(w => fileList.Select(s => s.ImgUuId).Contains(w.ImgUuId)).ToListAsync();
                fileList.ForEach(f => 
                {
                    
                    f.MID= _life.PersonNews.Where(w =>f.NoticeId.Contains(w.NoticeId) && w.PersonId == personId && w.ProjectInfoId==projectInfoIds).Select(s => s.MID).FirstOrDefault() ?? "0";
                    f.Integal= _life.IntegralRecords.Where(w => w.PersonId == personId && w.ProjectInfoId == projectInfoIds &&f.NoticeId.Contains(w.NoticeId)).Select(s => s.NoticeId).Any()? "1":"0";
                    f.Imgs = _life.FileImgs.Where(w => f.ImgUuId.Contains(w.ImgUuId)).Select(s => new FileloadImgViewModel
                    {
                        ImgUuId=s.ImgUuId,
                        ImgUrl=s.ImgUrl,
                        CreateTime=s.CreateTime,
                        VideoUrl=s.VideoUrl
                    }
                     
                    ).ToList();
                });
                //fileList.OrderBy(o => o.MID);
                return ResponseViewModel<object>.Create(Status.SUCCESS,Message.SUCCESS, fileList);
            }
            catch (Exception ex)
            {
                _logger.LogError("获取通知列表：" + ex.Message, ex);
                return ResponseViewModel<object>.Create(Status.ERROR, Message.ERROR);
            }
        }


        /// <summary>
        /// 获取通知、图文、视频信息
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        public async Task<ResponseViewModel<object>> GetNoticeInfo(string noticeId,string personId) 
        {
            if (string.IsNullOrEmpty(noticeId))
            {
                return ResponseViewModel<object>.Create(Status.FAIL, Message.FAIL);
            }
            try
            {

                //string noticeType = null;
                //switch (type)
                //{
                //    case 0: noticeType = "未读"; break;
                //    case 1: noticeType = "已读"; break;
                //    default:
                //        break;
                //}
                var now = DateTime.Now;
                var projInfo = await _life.ProjectPoints.Where(s => s.PersonId == personId).Select(s => s.ProjectInfoId).FirstOrDefaultAsync();
                    var noticeInfos = await _life.FileUploads.Where(f => f.DeleteMark == 0 && f.NoticeId == noticeId && f.ProjectInfoId== projInfo).FirstOrDefaultAsync();
                    var notice = new NoticeInfoViewModel
                    {
                        ProjectInfoId = noticeInfos.ProjectInfoId,
                        Title = noticeInfos.Title,
                        Subtitle = noticeInfos.Subtitle,
                        Content = noticeInfos.Content,
                        ContentLable = noticeInfos.ContentLable,
                        CreateDate = noticeInfos.CreateDate
                    };
                var news =  _life.PersonNews.Where(s => s.NoticeId == noticeId && s.PersonId==personId && s.ProjectInfoId==projInfo).FirstOrDefault();
                if (news == null)
                {
                    PersonNew person = new PersonNew 
                    {
                        MID="1",
                        ProjectInfoId=notice.ProjectInfoId,
                        NoticeId=noticeId,
                        PersonId=personId,
                        CreateTime=now
                    };
                    _life.PersonNews.Add(person);
                    await _life.SaveChangesAsync();
                }
                var mid = _life.PersonNews.Where(s => s.NoticeId == noticeId && s.PersonId == personId).FirstOrDefault();
                var result = new PersonNewViewModel
                {
                    Notices=notice,
                    MID= mid.MID
                };
                    //var NotMid =  _life.FileUploads.Where(s => s.NoticeId == noticeId && s.DeleteMark == 0).FirstOrDefault();
                    //NotMid.MID = "已读";
                    // _life.FileUploads.UpdateRange(NotMid);
                    //await _life.SaveChangesAsync();
                
                //notice.ImgUrl.ForEach(f => new NoticeInfoViewModel
                //{
                //   // ImgUrl =  _life.FileUploads.Where(f => f.DeleteMark == 0 && f.NoticeId == noticeId).Select(s => s.ImgUrl).ToList()
                //}) ;
                return ResponseViewModel<object>.Create(Status.SUCCESS,Message.SUCCESS, result);
            }
            catch (Exception ex)
            {
                _logger.LogError("获取通知信息：" + ex.Message, ex);
                return ResponseViewModel<object>.Create(Status.ERROR, Message.ERROR);
            }
        }


        /// <summary>
        /// 获取通知信息提示数量
        /// </summary>
        /// <param name="type"></param>
        /// <param name="projectInfoIds"></param>
        /// <returns></returns>
        public async Task<ResponseViewModel<object>> GetNoticeCount( string projectInfoIds,string personId) 
        {
            if (string.IsNullOrEmpty(projectInfoIds))
            {
                return ResponseViewModel<object>.Create(Status.FAIL, Message.FAIL);
            }
            try
            {

                List<string> list = new List<string>()
                {
                    "通知","图文","视频"
                };
                var notids = await _life.FileUploads.Where(s => s.DeleteMark == 0 && projectInfoIds==s.ProjectInfoId).ToListAsync();
                var news =await _life.PersonNews.Where(s => notids.Select(s=>s.NoticeId).Contains(s.NoticeId) && s.MID == "1" && s.PersonId == personId && projectInfoIds.Contains(s.ProjectInfoId)).ToListAsync();
                                
                var data1 = from a in _life.FileUploads
                            join b in _life.PersonNews
                            on new { a.NoticeId, a.ProjectInfoId } equals new { b.NoticeId, b.ProjectInfoId }
                            where b.PersonId == personId && projectInfoIds.Contains(b.ProjectInfoId) && list.Contains(a.NoticeType)
                            select a;
                var data2 = await data1.GroupBy(f => f.NoticeType).Select(f => new { f.Key, count = f.Count() }).ToListAsync();
                var result = new NoticeTyCountViewModel()
                {
                    NoticeCount = notids.Count(s => s.NoticeType =="通知")-(data2.FirstOrDefault(s=>s.Key=="通知")?.count??0),
                    ImgTxtCount = notids.Count(s => s.NoticeType == "图文") - (data2.FirstOrDefault(s => s.Key == "图文")?.count ?? 0),
                    VideoCount = notids.Count(s => s.NoticeType == "视频") - (data2.FirstOrDefault(s => s.Key == "视频")?.count ?? 0),

                };



                //foreach (var item in list)
                //    {
                //        var data = await (from a in _life.FileUploads
                //                          join b in _life.PersonNews
                //                          on new { a.NoticeId, a.ProjectInfoId } equals new { b.NoticeId, b.ProjectInfoId }
                //                          where b.PersonId == personId && projectInfoIds.Contains(a.ProjectInfoId) && a.NoticeType == item
                //                          select new
                //                          {
                //                              b.PersonId,
                //                              b.NoticeId
                //                          }
                //               ).CountAsync();

                //    var NoticeCount = notids.Where(s => s.NoticeType == item).Count()-data;
                //    result.NoticeCount = NoticeCount;
                //}
                

                return ResponseViewModel<object>.Create(Status.SUCCESS,Message.SUCCESS, result);
            }
            catch (Exception ex)
            {
                _logger.LogError("获取通知数量：" + ex.Message, ex);
                return ResponseViewModel<object>.Create(Status.ERROR, Message.ERROR);
            }
        }

        /// <summary>
        /// 人员绑定项目
        /// </summary>
        /// <param name="projectInfoId"></param>
        /// <param name="personId"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public async Task<ResponseViewModel<object>> AddPersonProject(ProjectPersonViewModel viewModel) 
        {

            if (viewModel==null)
            {
                return ResponseViewModel<object>.Create(Status.ERROR,"缺少参数");
            }
            try
            {
                var now = DateTime.Now;
                //根据项目ID获取项目信息
                var projectInfo = _life.ProjectPoints.Where(s => s.ProjectInfoId == viewModel.projectInfoId).FirstOrDefault();
                //根据人员ID查询是否已经绑定项目
                var projectPerson = _life.ProjectPoints.Where(s =>s.PersonId== viewModel.personId).FirstOrDefault();
                var userinfos = await _life.LifeInUserInfos.Where(s => s.PersonId == viewModel.personId && s.DeleteMark == 0).FirstOrDefaultAsync();
                //人员是否绑定项目
                if (projectPerson!=null)
                {
                    if (viewModel.projectInfoId == projectPerson.ProjectInfoId)
                    {
                        return ResponseViewModel<object>.Create(Status.FAIL, "你已绑定当前项目");
                    }
                    //var projectInfos = _life.ProjectPoints.Where(s => s.PersonId == personId).FirstOrDefault();
                    var integlInfos = await _life.IntegralInfos.Where(s => s.PersonId == viewModel.personId).FirstOrDefaultAsync();

                    if (projectPerson.ProjectInfoId != viewModel.projectInfoId)
                    {

                        projectPerson.ProjectInfoId = viewModel.projectInfoId;
                        projectPerson.ProjectName = projectInfo.ProjectName;
                        projectPerson.PersonId = viewModel.personId;
                        projectPerson.PersonName = userinfos.UserName;
                        projectPerson.AllRanks = projectInfo.AllRanks;
                        projectPerson.ProjectRanks = projectInfo.ProjectRanks;

                        _life.ProjectPoints.Update(projectPerson);
                        integlInfos.ProjectInfoId = viewModel.projectInfoId;
                        integlInfos.CreateTime = now;
                        _life.IntegralInfos.Update(integlInfos);
                        await _life.SaveChangesAsync();
                    }
                    return ResponseViewModel<object>.Create(Status.SUCCESS, "更改项目绑定成功");
                }
                ProjectPoint project = new ProjectPoint
                {
                    ProjectInfoId = viewModel.projectInfoId,
                    ProjectName = projectInfo.ProjectName,
                    PersonId = viewModel.personId,
                    PersonName= userinfos.UserName,
                    AllRanks = projectInfo.AllRanks,
                    ProjectRanks = projectInfo.ProjectRanks
                };
                _life.ProjectPoints.Add(project);
                IntegralInfo integralInfo = new IntegralInfo 
                {
                    PersonId=viewModel.personId,
                    ProjectInfoId=viewModel.projectInfoId,
                    IntegralNums=0,
                    UnIntegralNums=0,
                    CreateTime= now
                };
                _life.IntegralInfos.Add(integralInfo);
                await _life.SaveChangesAsync();
                return ResponseViewModel<object>.Create(Status.SUCCESS,"绑定成功");
            }
            catch (Exception ex)
            {
                _logger.LogError("绑定项目：" + ex.Message, ex);
                return ResponseViewModel<object>.Create(Status.ERROR, Message.ERROR);
            }

        }

        #region 安全找茬小游戏程序

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="file"></param>
        /// /// <param name="nvcContractNumber"></param>
        /// /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseViewModel<string>> uploadImage(IFormFile file)
        {
            try
            {
                Stream fs = file.OpenReadStream();
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                Imagelist mModel = new Imagelist()
                {
                    Content = bytes,
                    StartX = 0,
                    StartY = 0,
                    EndX = 0,
                    EndY = 0,
                    ImageName = "0"
                };
                await _life.Imagelist.AddRangeAsync(mModel);
                _context.SaveChanges();
                return ResponseViewModel<string>.Create(Status.SUCCESS, Message.SUCCESS, "添加成功");


            }
            catch (Exception ex)
            {

                return ResponseViewModel<string>.Create(Status.ERROR, Message.ERROR);
            }
        }

        [HttpGet]
        public async Task<ResponseViewModel<List<Imagelist>>> GetMemoData()
        {

            try
            {
                var glist = await _life.Imagelist.ToListAsync();


                int j;
                int[] b = new int[15];
                Random r = new Random();
                for (j = 0; j < 15; j++)
                {
                    int i = r.Next(1, 20 + 1);
                    int num = 0;
                    for (int k = 0; k < j; k++)
                    {
                        if (b[k] == i)
                        {
                            num = num + 1;
                        }
                    }
                    if (num == 0)
                    {
                        b[j] = i;
                    }
                    else
                    {
                        j = j - 1;
                    }
                }
                for (int i = 0; i < b.Length; i++)
                {
                    int temp = b[i];
                    int x = i;
                    while ((x > 0) && (b[x - 1] > temp))
                    {
                        b[x] = b[x - 1];
                        --x;
                    }
                    b[x] = temp;
                }
                List<Imagelist> mlist = new List<Imagelist>(15);
                int a = 0;
                int c = 0;
                glist.ForEach(item =>
                {
                    a++;
                    if (c < 15)
                    {
                        if (a == b[c])
                        {
                            c++;
                            mlist.Add(glist[a]);
                        }
                    }

                });
                var count = mlist.Count();
                return ResponseViewModel<List<Imagelist>>.Create(Status.SUCCESS, Message.SUCCESS, mlist, count);

            }
            catch (Exception ex)
            {

                return ResponseViewModel<List<Imagelist>>.Create(Status.ERROR, Message.ERROR);
            }
        }
        #endregion

    }
}
