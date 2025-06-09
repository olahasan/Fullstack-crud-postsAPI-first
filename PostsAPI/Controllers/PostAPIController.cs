using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostAPIBusinessLayer;
using StudentAPIDataAccessLayer;

namespace PostsAPI.Controllers
{
    [Route("api/PostAPI")]
    [ApiController]
    public class PostAPIController : ControllerBase
    {
        [HttpGet("AllPosts", Name = "GetAllPosts")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //here we used StudentDTO
        public ActionResult<IEnumerable<PostDTO>> GetAllPosts() // Define a method to get all Posts.
        {
            List<PostDTO> PostList = PostBusiness.GetAllPosts();
            if (PostList.Count == 0)
            {
                //return NotFound("No Posts Found!"); //change it to solve problem in react project
                return Ok(new List<PostDTO>());
            }
            return Ok(PostList); // Returns the list of Posts.

        }




        //here we use HttpDelete method
        [HttpDelete("Delete/{id}", Name = "DeletePost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeletePost(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            if (PostBusiness.DeletePost(id))

                return Ok($"Post with ID {id} has been deleted.");
            else
                return NotFound($"Post with ID {id} not found. no rows deleted!");
        }



        [HttpPost("AddNewPost", Name = "AddPost")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PostDTO> AddPost(PostDTO newPostDTO)  
        {
            //we validate the data here
            if (newPostDTO == null || string.IsNullOrEmpty(newPostDTO.Title) || string.IsNullOrEmpty(newPostDTO.Description) || string.IsNullOrEmpty(newPostDTO.UserId))
            {
                return BadRequest("Invalid post data.");
            }


            PostBusiness post = new PostBusiness (new PostDTO(newPostDTO.Id, newPostDTO.Title, newPostDTO.Description, newPostDTO.UserId));
            post.Save();

            newPostDTO.Id = post.ID;

            //we return the DTO only not the full student object
            return Ok(newPostDTO);
            //return CreatedAtAction(nameof(GetPostById), new { id = newPostDTO.Id }, newPostDTO);
            //return CreatedAtRoute("GetPostById", new { id = newPostDTO.Id }, newPostDTO);

        }





        //here we use http put method for update
        [HttpPut("update/{id}", Name = "UpdatePost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PostDTO> UpdatePost(int id, PostDTO updatedPost)
        {
            if (id < 1 || updatedPost == null || string.IsNullOrEmpty(updatedPost.Title) || string.IsNullOrEmpty(updatedPost.Description) || string.IsNullOrEmpty(updatedPost.UserId))
            {
                return BadRequest("Invalid student data.");
            }

            PostBusiness Post = PostBusiness.Find(id);


            if (Post == null)
            {
                return NotFound($"Post with ID {id} not found.");
            }


            Post.Title = updatedPost.Title;
            Post.Description = updatedPost.Description;
            Post.UserId = updatedPost.UserId;


            Post.Save();

            //we return the DTO not the full student object.
            return Ok(Post.PDTO);

        }




        [HttpGet("GetPostById/{id}", Name = "GetPostById")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<PostDTO> GetPostById(int id)
        {

            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            PostBusiness Post = PostBusiness.Find(id);

            if (Post == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            //here we get only the DTO object to send it back.
            PostDTO PDTO = Post.PDTO;

            //we return the DTO not the student object.
            return Ok(PDTO);

        }

    }
}
