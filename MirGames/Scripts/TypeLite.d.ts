﻿
 
 


declare module MirGames.Infrastructure.Commands {
interface Command {
}
interface Command1 extends MirGames.Infrastructure.Commands.Command {
}
interface Command1 extends MirGames.Infrastructure.Commands.Command {
}
}
declare module MirGames.Infrastructure.Queries {
interface Query {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface SingleItemQuery1 extends MirGames.Infrastructure.Queries.Query1 {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface SingleItemQuery1 extends MirGames.Infrastructure.Queries.Query1 {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface SingleItemQuery1 extends MirGames.Infrastructure.Queries.Query1 {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface SingleItemQuery1 extends MirGames.Infrastructure.Queries.Query1 {
}
interface SingleItemQuery1 extends MirGames.Infrastructure.Queries.Query1 {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface SingleItemQuery1 extends MirGames.Infrastructure.Queries.Query1 {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface SingleItemQuery1 extends MirGames.Infrastructure.Queries.Query1 {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
}
declare module MirGames.Domain.Tools.Queries {
interface GetEventLogQuery extends MirGames.Infrastructure.Queries.Query1 {
  LogType: MirGames.Infrastructure.Logging.EventLogType;
  UserName: string;
  Source: string;
  Message: string;
  From: Date;
  To: Date;
}
}
declare module MirGames.Domain.Tools.ViewModels {
interface EventLogViewModel {
  Id: number;
  EventLogType: MirGames.Infrastructure.Logging.EventLogType;
  Login: string;
  Message: string;
  Source: string;
  Details: string;
  Date: Date;
}
}
declare module MirGames.Domain.Topics.Commands {
interface DeleteCommentCommand extends MirGames.Infrastructure.Commands.Command {
  CommentId: number;
}
interface EditCommentCommand extends MirGames.Infrastructure.Commands.Command {
  CommentId: number;
  Text: string;
  Attachments: number[];
}
interface PostNewCommentCommand extends MirGames.Infrastructure.Commands.Command1 {
  TopicId: number;
  Text: string;
  Attachments: number[];
}
}
declare module MirGames.Domain.Topics.Queries {
interface GetCommentForEditQuery extends MirGames.Infrastructure.Queries.SingleItemQuery1 {
  CommentId: number;
}
interface GetCommentByIdQuery extends MirGames.Infrastructure.Queries.SingleItemQuery1 {
  CommentId: number;
}
}
declare module MirGames.Domain.Topics.ViewModels {
interface CommentForEditViewModel {
  Id: number;
  SourceText: string;
}
interface CommentViewModel {
  Text: string;
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  CreationDate: Date;
  UpdatedDate: Date;
  Id: number;
  TopicId: number;
  TopicTitle: string;
  CanBeEdited: boolean;
  CanBeDeleted: boolean;
}
interface TopicForEditViewModel {
  Id: number;
  Title: string;
  Tags: string;
  Text: string;
}
interface TopicViewModel {
  Id: number;
  Title: string;
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  CommentsCount: number;
  Text: string;
  Comments: MirGames.Domain.Topics.ViewModels.CommentViewModel[];
  CanBeEdited: boolean;
  CanBeDeleted: boolean;
  CanBeCommented: boolean;
  Tags: string[];
  TagsList: string;
  CreationDate: Date;
}
}
declare module MirGames.Domain.Users.ViewModels {
interface AuthorViewModel {
  AvatarUrl: string;
  Login: string;
  Id: number;
}
interface OAuthProviderViewModel {
  ProviderId: number;
  ProviderName: string;
  DisplayName: string;
  IsLinked: boolean;
}
interface OnlineUserViewModel {
  AvatarUrl: string;
  Login: string;
  Id: number;
  SessionDate: Date;
  LastRequestDate: Date;
  Tags: string[];
}
interface CurrentUserViewModel {
  AvatarUrl: string;
  Login: string;
  Id: number;
  Name: string;
  TimeZone: string;
  IsActivated: boolean;
  Settings: any;
}
interface UserClaimViewModel {
  Type: string;
  Value: string;
}
interface UserListItemViewModel {
  AvatarUrl: string;
  Login: string;
  Id: number;
  Name: string;
  RegistrationDate: Date;
  LastVisit: Date;
  Location: string;
  UserRating: number;
  IsOnline: boolean;
}
interface UserViewModel {
  AvatarUrl: string;
  Login: string;
  Id: number;
  Name: string;
  About: string;
  Location: string;
  Rating: number;
  Birthday: Date;
  RegistrationDate: Date;
  LastVisitDate: Date;
  CanBeDeleted: boolean;
  CanReceiveMessage: boolean;
  WallRecordCanBeAdded: boolean;
}
interface UserWallRecordViewModel {
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  DateAdd: Date;
  Text: string;
}
}
declare module MirGames.Domain.Users.Commands {
interface AddOnlineUserTagCommand extends MirGames.Infrastructure.Commands.Command {
  Tag: string;
  ExpirationTime: number;
}
interface DetachOAuthProviderCommand extends MirGames.Infrastructure.Commands.Command {
  ProviderName: string;
}
interface RequestPasswordRestoreCommand extends MirGames.Infrastructure.Commands.Command {
  EmailOrLogin: string;
  NewPasswordHash: string;
}
interface RemoveOnlineUserTagCommand extends MirGames.Infrastructure.Commands.Command {
  Tag: string;
}
interface ResendActivationCommand extends MirGames.Infrastructure.Commands.Command {
}
interface SaveAccountSettingsCommand extends MirGames.Infrastructure.Commands.Command {
  Settings: any;
}
interface SaveUserProfileCommand extends MirGames.Infrastructure.Commands.Command {
  Name: string;
  Location: string;
  Birthday: Date;
  Company: string;
  Career: string;
  About: string;
  GitHubLink: string;
  BitBucketLink: string;
  HabrahabrLink: string;
}
interface SetUserAvatarCommand extends MirGames.Infrastructure.Commands.Command {
  AvatarAttachmentId: number;
}
}
declare module System.Collections.Generic {
interface KeyValuePair2 {
  Key: any;
  Value: any;
}
}
declare module MirGames.Domain.Users.Queries {
interface GetOAuthProvidersQuery extends MirGames.Infrastructure.Queries.Query1 {
}
}
declare module MirGames.Domain.Forum.Commands {
interface DeleteForumPostCommand extends MirGames.Infrastructure.Commands.Command {
  PostId: number;
}
interface DeleteForumTopicCommand extends MirGames.Infrastructure.Commands.Command {
  TopicId: number;
}
interface UpdateForumPostCommand extends MirGames.Infrastructure.Commands.Command {
  Attachments: number[];
  Text: string;
  TopicTitle: string;
  TopicsTags: string;
  PostId: number;
}
interface ReplyForumTopicCommand extends MirGames.Infrastructure.Commands.Command1 {
  Attachments: number[];
  Text: string;
  TopicId: number;
}
}
declare module MirGames.Domain.Forum.Queries {
interface GetForumTopicPostsQuery extends MirGames.Infrastructure.Queries.Query1 {
  TopicId: number;
  LoadStartPost: boolean;
}
interface GetForumPostForEditQuery extends MirGames.Infrastructure.Queries.SingleItemQuery1 {
  PostId: number;
}
interface GetForumPostQuery extends MirGames.Infrastructure.Queries.SingleItemQuery1 {
  PostId: number;
}
interface GetForumTopicQuery extends MirGames.Infrastructure.Queries.SingleItemQuery1 {
  TopicId: number;
}
}
declare module MirGames.Domain.Forum.ViewModels {
interface ForumPostForEditViewModel {
  PostId: number;
  SourceText: string;
  TopicTitle: string;
  TopicTags: string;
  CanChangeTitle: boolean;
  CanChangeTags: boolean;
}
interface ForumPostViewModel {
  PostId: number;
  TopicTitle: string;
  Text: string;
  CreatedDate: Date;
  UpdatedDate: Date;
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  AuthorIP: string;
  IsHidden: boolean;
  TopicId: number;
}
interface ForumPostsListItemViewModel {
  PostId: number;
  Text: string;
  CreatedDate: Date;
  UpdatedDate: Date;
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  AuthorIP: string;
  IsHidden: boolean;
  TopicId: number;
  Index: number;
  IsRead: boolean;
  FirstUnread: boolean;
  IsFirstPost: boolean;
  CanBeEdited: boolean;
  CanBeDeleted: boolean;
}
interface ForumTopicsListItemViewModel {
  TopicId: number;
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  AuthorIp: string;
  LastPostAuthor: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  Title: string;
  TagsList: string;
  Tags: string[];
  CreatedDate: Date;
  UpdatedDate: Date;
  PostsCount: number;
  UnreadPostsCount: number;
  IsRead: boolean;
}
interface ForumTopicViewModel {
  TopicId: number;
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  AuthorIp: string;
  Title: string;
  TagsList: string;
  StartPost: MirGames.Domain.Forum.ViewModels.ForumPostsListItemViewModel;
  Tags: string[];
  CreatedDate: Date;
  UpdatedDate: Date;
  CanBeAnswered: boolean;
  CanBeEdited: boolean;
  CanBeDeleted: boolean;
  IsRead: boolean;
}
}
declare module MirGames.Domain.Chat.Commands {
interface PostChatMessageCommand extends MirGames.Infrastructure.Commands.Command1 {
  Message: string;
  Attachments: number[];
}
interface UpdateChatMessageCommand extends MirGames.Infrastructure.Commands.Command {
  MessageId: number;
  Message: string;
  Attachments: number[];
}
}
declare module MirGames.Domain.Chat.Queries {
interface GetChatMessageForEditQuery extends MirGames.Infrastructure.Queries.SingleItemQuery1 {
  MessageId: number;
}
interface GetChatMessagesQuery extends MirGames.Infrastructure.Queries.Query1 {
  LastIndex: number;
  FirstIndex: number;
}
}
declare module MirGames.Domain.Chat.ViewModels {
interface ChatMessageViewModel {
  MessageId: number;
  Text: string;
  CanBeEdited: boolean;
  CanBeDeleted: boolean;
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  CreatedDate: Date;
  UpdatedDate: Date;
}
interface ChatMessageForEditViewModel {
  MessageId: number;
  SourceText: string;
}
}
declare module MirGames.Domain.Attachments.ViewModels {
interface AttachmentViewModel {
  AttachmentId: number;
  ContentType: string;
  UserId: number;
  CreatedDate: Date;
  FileName: string;
  FilePath: string;
  AttachmentUrl: string;
  FileSize: number;
  EntityId: number;
  EntityType: string;
  IsImage: boolean;
}
}
declare module MirGames.Domain.Wip.Commands {
interface CreateNewWipProjectCommand extends MirGames.Infrastructure.Commands.Command1 {
  Title: string;
  Alias: string;
  Tags: string;
  RepositoryType: string;
  LogoAttachmentId: number;
  Attachments: number[];
  Description: string;
}
}
declare module MirGames.Domain.Wip.Queries {
interface GetIsProjectNameUniqueQuery extends MirGames.Infrastructure.Queries.SingleItemQuery1 {
  Alias: string;
}
}
declare module MirGames.Domain.Wip.ViewModels {
enum WipProjectRepositoryItemType {
  Other = 0,
  File = 1,
  Directory = 2
}
interface WipProjectRepositoryItemViewModel {
  Path: string;
  Name: string;
  ItemType: MirGames.Domain.Wip.ViewModels.WipProjectRepositoryItemType;
}
interface WipProjectCommitViewModel {
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  Message: string;
  Date: Date;
}
interface WipProjectFileViewModel {
  FileName: string;
  UpdatedDate: Date;
  Content: string;
  IsPreview: boolean;
}
interface WipProjectViewModel {
  ProjectId: number;
  Title: string;
  Alias: string;
  Description: string;
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  LogoUrl: string;
  CreationDate: Date;
  UpdatedDate: Date;
  Version: string;
  Tags: string[];
  VotesCount: number;
  Votes: number;
  FollowersCount: number;
  RepositoryUrl: string;
}
}
declare module MirGames.Infrastructure.Logging {
enum EventLogType {
  Error = 0,
  Warning = 1,
  Information = 2,
  Verbose = 3
}
}