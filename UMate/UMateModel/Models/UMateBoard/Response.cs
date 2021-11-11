using System;
using System.Collections.Generic;
using UMateModel.Entities.UMateBoard;

namespace UMateModel.Models.UMateBoard
{
    // bulkInsert response
    public class BoardPostResponse : CommonResponse
    {
        public Board Board { get; set; }
    }

    public class CategoryPostResponse : CommonResponse
    {
        public Category Category { get; set; }
    }

    public class PostPostResponse : CommonResponse
    {
        public Post Post { get; set; }
    }

    public class CommentPostResponse : CommonResponse
    {
        public Comment Comment { get; set; }
    }

    public class ImagePostResponse : CommonResponse
    {
        public PostImage Image { get; set; }
    }




    // fetch
    public class BoardListResponse : CommonResponse
    {
        public List<BoardDto> List { get; set; }
    }

    public class BoardListResponse<T>
    {

    }

    public class BoardResponse : CommonResponse
    {
        public BoardDto Board { get; set; }
    }

    public class PostListResponse<T>
    {

    }

    public class PostListResponse : CommonResponse
    {
        public int TotalCount { get; set; }
        public List<PostListDto> List { get; set; }
    }

    public class ImageListResponse<T>
    {

    }

    public class ImageListResponse : CommonResponse
    {
        public List<PostImage> List { get; set; }
    }

    public class ScrapPostListResponse<T>
    {

    }

    public class ScrapPostListResponse : CommonResponse
    {
        public int TotalCount { get; set; }
        public List<PostListDto> List { get; set; }
    }

    public class MyCommentListResponse<T>
    {

    }

    public class MyCommentListResponse : CommonResponse
    {
        public int TotalCount { get; set; }
        public List<PostListDto> List { get; set; }
    }

    public class MyPostListResponse<T>
    {

    }

    public class MyPostListResponse : CommonResponse
    {
        public int TotalCount { get; set; }
        public List<PostListDto> List { get; set; }
    }

    public class PostResponse : CommonResponse
    {
        public PostDto Post { get; set; }
        public bool isLiked { get; set; }
        public bool isScrapped { get; set; }
        public int scrapPostId { get; set; }
    }

    public class CommentListResponse<T>
    {

    }

    public class CommentListResponse : CommonResponse
    {
        public int LastId { get; set; }
        public List<Comment> List { get; set; }
    }

    public class LikeCommentListResponse<T>
    {

    }

    public class LikeCommentListResponse : CommonResponse
    {
        public List<LikeComment> List { get; set; }
    }

    public class LikePostListResponse<T>
    {

    }

    public class LikePostListResponse : CommonResponse
    {
        public List<LikePost> List { get; set; }
    }



    // post
    // 게시물 저장 응D

    public class SavePostResponse : CommonResponse
    {
        public PostDto Post { get; set; }
    }

    // 게시물 이미지 저장 응답
    public class SaveImageResponse : CommonResponse
    {
        public List<PostImage> List { get; set; }
    }


    public class SaveCommentResponse : CommonResponse
    {
        public Comment Comment { get; set; }
    }

    public class SaveLikeCommentResponse : CommonResponse
    {
        public LikeComment LikeComment { get; set; }
    }

    public class SaveScrapPostResponse : CommonResponse
    {
        public ScrapPost scrapPost { get; set; }
    }










    //
    //
    // 강의 정보
    public class LectureInfoListResponse<T>
    {

    }

    public class LectureInfoListResponse : CommonResponse
    {
        public int TotalCount { get; set; }
        public List<LectureInfoListDto> List { get; set; }
    }


    public class LectureInfoDetailResponse<T>
    {

    }

    public class LectureInfoDetailResponse : CommonResponse
    {
        public LectureInfoDetailDto LectureInfo { get; set; }
    }

    public class LectureInfoResponse : CommonResponse
    {
        public LectureInfo LectureInfo { get; set; }
    }

    public class LectureReviewListResponse<T>
    {

    }

    public class LectureReviewListResponse : CommonResponse
    {
        public List<LectureReview> LectureReviews { get; set; }
    }

    public class TestInfoListResponse<T>
    {

    }

    public class TestInfoListResponse : CommonResponse
    {
        public List<TestInfo> TestInfos { get; set; }
    }


    // BulkInsert, POST
    public class LectureInfoPostResponse : CommonResponse
    {
        public LectureInfo LectureInfo { get; set; }
    }

    public class LectureReviewPostResponse : CommonResponse
    {
        public LectureReview LectureReview { get; set; }
    }

    public class TestInfoPostResponse : CommonResponse
    {
        public TestInfoDto TestInfo { get; set; }
        public List<Example> Examples { get; set; }
    }

    public class ExamplePostResponse : CommonResponse
    {
        public Example Example { get; set; }
    }
}
