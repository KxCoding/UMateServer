using System;
using System.Collections.Generic;
using UMateModel.Entities.UMateBoard;

namespace UMateModel.Models.UMateBoard
{
    /// <summary>
    /// BULKINSERT RESPONSE
    ///
    /// 벌크인서트 할 때 제대로 들어갔는지 확인 할 때 사용
    /// </summary>
    /// 게시판 데이터
    public class BoardPostResponse : CommonResponse
    {
        public Board Board { get; set; }
    }

    /// 카테고리 데이터
    public class CategoryPostResponse : CommonResponse
    {
        public Category Category { get; set; }
    }

    /// 게시글 데이터
    public class PostPostResponse : CommonResponse
    {
        public Post Post { get; set; }
    }

    /// 댓글 데이터
    public class CommentPostResponse : CommonResponse
    {
        public CommentDto Comment { get; set; }
    }

    /// 게시글 이미지 데이터
    public class ImagePostResponse : CommonResponse
    {
        public PostImage Image { get; set; }
    }


    /// <summary>
    /// 클라이언트에 서버데이터를 불러올 때 사용
    /// </summary>
    /// 게시판 목록
    public class BoardListResponse : CommonResponse
    {
        public List<BoardDto> List { get; set; }
    }

    public class BoardListResponse<T>
    {

    }

    /// 게시판 하나의 정보
    public class BoardResponse : CommonResponse
    {
        public BoardDto Board { get; set; }
    }

    /// 게시판에 해당하는 게시글 목록
    public class PostListResponse : CommonResponse
    {
        public int TotalCount { get; set; }
        public List<PostListDto> List { get; set; }
    }

    public class PostListResponse<T>
    {

    }

    /// 게시글에 포함된 이미지 목록
    public class ImageListResponse : CommonResponse
    {
        public List<PostImage> List { get; set; }
    }

    public class ImageListResponse<T>
    {

    }

    /// 사용자가 스크랩한 게시글 목록
    public class ScrapPostListResponse : CommonResponse
    {
        public int TotalCount { get; set; }
        public List<PostListDto> List { get; set; }
    }

    public class ScrapPostListResponse<T>
    {

    }

    /// 사용자가 댓글 단 글 목록
    public class MyCommentListResponse : CommonResponse
    {
        public int TotalCount { get; set; }
        public List<PostListDto> List { get; set; }
    }

    public class MyCommentListResponse<T>
    {

    }

    /// 사용자가 작성한 게시글 목록
    public class MyPostListResponse : CommonResponse
    {
        public int TotalCount { get; set; }
        public List<PostListDto> List { get; set; }
    }

    public class MyPostListResponse<T>
    {

    }

    /// 게시글 세부정보
    public class PostDetailResponse : CommonResponse
    {
        public PostDetailDto Post { get; set; }
        public bool isLiked { get; set; }
        public bool isScrapped { get; set; }
        public int scrapPostId { get; set; }
    }

    /// 게시글에 해당하는 댓글 목록
    public class CommentListResponse : CommonResponse
    {
        public int LastId { get; set; }
        public List<CommentDto> List { get; set; }
    }

    public class CommentListResponse<T>
    {

    }

    /// 사용자가 댓글 좋아요한 목록
    public class LikeCommentListResponse : CommonResponse
    {
        public List<LikeComment> List { get; set; }
    }

    public class LikeCommentListResponse<T>
    {

    }

    /// 사용자가 좋아요한 게시글 목록
    public class LikePostListResponse : CommonResponse
    {
        public List<LikePost> List { get; set; }
    }

    public class LikePostListResponse<T>
    {

    }

    /// 강의 정보 목록
    public class LectureInfoListResponse : CommonResponse
    {
        public int TotalCount { get; set; }
        public List<LectureInfoListDto> List { get; set; }
    }

    public class LectureInfoListResponse<T>
    {

    }

    /// 강의 세부 정보
    public class LectureInfoDetailResponse : CommonResponse
    {
        public LectureInfoDetailDto LectureInfo { get; set; }
    }

    public class LectureInfoDetailResponse<T>
    {

    }

    /// 강의평 목록
    public class LectureReviewListResponse : CommonResponse
    {
        public List<LectureReview> LectureReviews { get; set; }
    }

    public class LectureReviewListResponse<T>
    {

    }
   
    /// 시험 정보 목록
    public class TestInfoListResponse : CommonResponse
    {
        public List<TestInfo> TestInfos { get; set; }
    }

    public class TestInfoListResponse<T>
    {

    }


    /// <summary>
    /// 게시판 POST
    /// </summary>
    /// 게시물 저장 응답 데이터
    public class SavePostResponse : CommonResponse
    {
        public PostDetailDto Post { get; set; }
    }

    /// 게시물 이미지 저장 응답 데이터
    public class SaveImageResponse : CommonResponse
    {
        public List<PostImage> List { get; set; }
    }

    /// 댓글 저장 응답 데이터
    public class SaveCommentResponse : CommonResponse
    {
        public Comment Comment { get; set; }
    }

    /// 댓글 좋아요 응답 데이터
    public class SaveLikeCommentResponse : CommonResponse
    {
        public LikeComment LikeComment { get; set; }
    }

    /// 게시물 스크랩 응답 데이터
    public class SaveScrapPostResponse : CommonResponse
    {
        public ScrapPost scrapPost { get; set; }
    }


    /// <summary>
    /// 강의 정보 POST
    /// </summary>
    /// BulkInsert, POST 공통 사용
    /// 강의 정보 저장 응답 데이터
    public class LectureInfoPostResponse : CommonResponse
    {
        public LectureInfo LectureInfo { get; set; }
    }

    /// 강의평 저장 응답 데이터
    public class LectureReviewPostResponse : CommonResponse
    {
        public LectureReview LectureReview { get; set; }
    }

    /// 시험정보 저장 응답 데이터
    public class TestInfoPostResponse : CommonResponse
    {
        public TestInfoDto TestInfo { get; set; }
        public List<Example> Examples { get; set; }
    }

    /// 시험정보의 문제예시 저장 응답데이터 
    public class ExamplePostResponse : CommonResponse
    {
        public Example Example { get; set; }
    }
}
