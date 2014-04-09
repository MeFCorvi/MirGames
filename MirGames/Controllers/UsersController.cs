﻿namespace MirGames.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;

    using MirGames.Domain.Forum.Queries;
    using MirGames.Domain.Forum.ViewModels;
    using MirGames.Domain.Topics.Queries;
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Filters;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Models;

    /// <summary>
    /// The topics controller.
    /// </summary>
    public class UsersController : AppController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public UsersController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
            : base(queryProcessor, commandProcessor)
        {
        }

        /// <inheritdoc />
        public ActionResult Index(int page = 1, string searchString = null)
        {
            var usersQuery = new GetUsersQuery
            {
                SortBy = GetUsersQuery.SortType.LastVisit,
                SearchString = searchString
            };

            this.PageData["action"] = "Index";

            return this.ShowUsersList(usersQuery, "Active", "Активные пользователи", page);
        }

        /// <inheritdoc />
        public ActionResult Top(int page = 1, string searchString = null)
        {
            var usersQuery = new GetUsersQuery
            {
                SortBy = GetUsersQuery.SortType.Rating,
                SearchString = searchString
            };

            this.PageData["action"] = "Top";

            return this.ShowUsersList(usersQuery, "Top", "Рейтинг пользователей", page);
        }

        /// <inheritdoc />
        [Authorize(Roles = "Administrator")]
        public ActionResult NotActivated(int page = 1, string searchString = null)
        {
            var usersQuery = new GetUsersQuery
            {
                SortBy = GetUsersQuery.SortType.LastVisit,
                Filter = GetUsersQuery.UserTypes.NotActivated,
                SearchString = searchString
            };

            this.PageData["action"] = "NotActivated";

            return this.ShowUsersList(usersQuery, "NotActivated", "Неактивированные пользователи", page);
        }

        /// <inheritdoc />
        public ActionResult Online(int page = 1, string searchString = null)
        {
            var usersQuery = new GetUsersQuery
            {
                SortBy = GetUsersQuery.SortType.Login,
                Filter = GetUsersQuery.UserTypes.Activated | GetUsersQuery.UserTypes.Online,
                SearchString = searchString
            };

            this.PageData["action"] = "Online";

            return this.ShowUsersList(usersQuery, "Online", "Пользователи онлайн", page);
        }

        /// <summary>
        /// The profile action.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>The action result.</returns>
        public new ActionResult Profile(int userId)
        {
            var user = this.QueryProcessor.Process(new GetUserByIdQuery { UserId = userId });
            var wallRecords = this.QueryProcessor.Process(new GetUserWallRecordsQuery { UserId = userId }, new PaginationSettings(0, 20));

            if (user == null)
            {
                return this.HttpNotFound();
            }

            this.PageData["userId"] = userId;
            this.ViewBag.SectionMode = "Profile";

            return this.View(
                "Profile",
                new ProfileViewModel { User = user, WallRecords = wallRecords });
        }

        /// <inheritdoc />
        public ActionResult Topics(int userId, int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            var paginationSettings = new PaginationSettings(page - 1, 20);
            var user = this.QueryProcessor.Process(new GetUserByIdQuery { UserId = userId });
            
            var query = new GetTopicsByUserQuery { UserId = userId };
            var topics = this.QueryProcessor.Process(query, paginationSettings);
            var topicsCount = this.QueryProcessor.GetItemsCount(query);

            this.PageData["userId"] = userId;
            this.ViewBag.SectionMode = "Topics";

            this.ViewBag.Pagination = new PaginationViewModel(
                paginationSettings,
                topicsCount,
                p => this.Url.Action("Topics", "Users", new { page = p }));

            return this.View(
                "Topics",
                new TopicsPageViewModel { User = user, Topics = topics });
        }

        /// <inheritdoc />
        public ActionResult Comments(int userId, int page = 1)
        {
            var user = this.QueryProcessor.Process(new GetUserByIdQuery { UserId = userId });
            if (page < 1)
            {
                page = 1;
            }

            var paginationSettings = new PaginationSettings(page - 1, 20);
            var query = new GetCommentsQuery { AuthorId = userId };
            var comments = this.QueryProcessor.Process(query, paginationSettings);
            var commentsCount = this.QueryProcessor.GetItemsCount(query);

            this.ViewBag.Pagination = new PaginationViewModel(
                paginationSettings,
                commentsCount,
                p => this.Url.Action("Comments", "Users", new { page = p }));

            this.PageData["userId"] = userId;
            this.ViewBag.SectionMode = "Comments";

            return this.View(
                "Comments",
                new CommentsPageViewModel { User = user, Comments = comments });
        }

        /// <inheritdoc />
        public ActionResult Forum(int userId, int page = 1)
        {
            var user = this.QueryProcessor.Process(new GetUserByIdQuery { UserId = userId });
            if (page < 1)
            {
                page = 1;
            }

            var paginationSettings = new PaginationSettings(page - 1, 20);
            var query = new GetForumPostsQuery { AuthorId = userId };
            var posts = this.QueryProcessor.Process(query, paginationSettings);
            var postsCount = this.QueryProcessor.GetItemsCount(query);

            this.ViewBag.Pagination = new PaginationViewModel(
                paginationSettings,
                postsCount,
                p => this.Url.Action("Forum", "Users", new { page = p }));

            this.PageData["userId"] = userId;
            this.ViewBag.SectionMode = "Forum";

            return this.View(
                "ForumPosts",
                new ForumPageViewModel { User = user, Posts = posts });
        }

        /// <summary>
        /// The profile action.
        /// </summary>
        /// <returns>The action result.</returns>
        [Authorize(Roles = "User, ReadOnlyUser")]
        public ActionResult Settings()
        {
            var user = this.QueryProcessor.Process(new GetUserByIdQuery { UserId = this.CurrentUser.Id });
            var authProviders = this.QueryProcessor.Process(new GetOAuthProvidersQuery());

            this.ViewBag.SectionMode = "Settings";
            this.PageData["timeZone"] = this.CurrentUser.TimeZone;
            this.PageData["user"] = user;
            this.PageData["oauthProviders"] = authProviders;

            return this.View(user);
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [AjaxOnly]
        [AntiForgery]
        public ActionResult DeleteUser(DeleteUserCommand command)
        {
            Contract.Requires(command != null);

            this.CommandProcessor.Execute(command);
            return this.Json(new { result = "ok" });
        }

        /// <summary>
        /// Posts new wall record.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [AjaxOnly]
        [AntiForgery]
        public ActionResult NewWallRecord(PostWallRecordCommand command)
        {
            Contract.Requires(command != null);

            var recordId = this.CommandProcessor.Execute(command);
            var wallRecord = this.QueryProcessor.Process(new GetWallRecordByIdQuery { WallRecordId = recordId });
            return this.PartialView("_WallRecord", wallRecord);
        }

        /// <inheritdoc />
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.CurrentSection = "Users";
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Shows the users list.
        /// </summary>
        /// <param name="usersQuery">The users query.</param>
        /// <param name="sectionMode">The section mode.</param>
        /// <param name="sectionTitle">The section title.</param>
        /// <param name="page">The page.</param>
        /// <returns>
        /// The users list.
        /// </returns>
        private ActionResult ShowUsersList(GetUsersQuery usersQuery, string sectionMode, string sectionTitle, int page)
        {
            if (page < 1)
            {
                page = 1;
            }

            var paginationSettings = new PaginationSettings(page - 1, 20);
            var users = this.QueryProcessor.Process(usersQuery, paginationSettings);
            var usersCount = this.QueryProcessor.GetItemsCount(usersQuery);

            this.ViewBag.UsersCount = usersCount;
            this.ViewBag.SectionMode = sectionMode;
            this.ViewBag.SectionTitle = sectionTitle;
            this.ViewBag.Pagination = new PaginationViewModel(
                paginationSettings,
                usersCount,
                p => this.Url.Action("Index", "Users", new { page = p }));

            this.PageData["searchString"] = usersQuery.SearchString;

            return this.View("Index", users);
        }

        /// <summary>
        /// The profile view model.
        /// </summary>
        public class ProfileViewModel
        {
            /// <summary>
            /// Gets or sets the user.
            /// </summary>
            public UserViewModel User { get; set; }

            /// <summary>
            /// Gets or sets the topics.
            /// </summary>
            public IEnumerable<UserWallRecordViewModel> WallRecords { get; set; }
        }

        /// <summary>
        /// The topics page view model.
        /// </summary>
        public class TopicsPageViewModel
        {
            /// <summary>
            /// Gets or sets the user.
            /// </summary>
            public UserViewModel User { get; set; }

            /// <summary>
            /// Gets or sets the topics.
            /// </summary>
            public IEnumerable<TopicsListItem> Topics { get; set; }
        }

        /// <summary>
        /// The topics page view model.
        /// </summary>
        public class CommentsPageViewModel
        {
            /// <summary>
            /// Gets or sets the user.
            /// </summary>
            public UserViewModel User { get; set; }

            /// <summary>
            /// Gets or sets the comments.
            /// </summary>
            public IEnumerable<CommentViewModel> Comments { get; set; }
        }

        /// <summary>
        /// The topics page view model.
        /// </summary>
        public class ForumPageViewModel
        {
            /// <summary>
            /// Gets or sets the user.
            /// </summary>
            public UserViewModel User { get; set; }

            /// <summary>
            /// Gets or sets the comments.
            /// </summary>
            public IEnumerable<ForumPostViewModel> Posts { get; set; }
        }
    }
}