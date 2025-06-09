
using StudentAPIDataAccessLayer;

namespace PostAPIBusinessLayer
{
    public class PostBusiness
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public PostDTO PDTO
        {
            get { return (new PostDTO(this.ID, this.Title, this.Description, this.UserId)); }
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }

        public PostBusiness(PostDTO PDTO, enMode cMode = enMode.AddNew)

        {
            this.ID = PDTO.Id;
            this.Title = PDTO.Title;
            this.Description = PDTO.Description;
            this.UserId = PDTO.UserId;

            Mode = cMode;
        }

      

        public static List<PostDTO> GetAllPosts()
        {
            return PostsDataAccess.GetAllPosts();
        }


        public static bool DeletePost(int ID)
        {
            return PostsDataAccess.DeletePost(ID);
        }


        private bool _AddNewPost()
        {
            //call DataAccess Layer 

            this.ID = PostsDataAccess.AddPost(PDTO);

            return (this.ID != -1);
        }




        private bool _UpdatePost()
        {
            return  PostsDataAccess.UpdatePost(PDTO);
        }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPost())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdatePost();

            }

            return false;
        }



        public static PostBusiness Find(int ID)
        {

            PostDTO PDTO = PostsDataAccess.GetPostById(ID);

            if (PDTO != null)
            //we return new object of that student with the right data
            {

                return new PostBusiness(PDTO, enMode.Update);
            }
            else
                return null;
        }
    }
}
