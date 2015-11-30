
   function checkLogin(){
        if (Parse.User.current()){
            //console.log("Logged In"+Parse.User.current().get("username"));
            $("#current-user").html("user: "+Parse.User.current().get("username"));
        }
          else{
            $("#current-user").html("");
          }
      }
       
       
      
      /*User LogOut*/
       $("#logout").submit(function(event){
            event.preventDefault();
          
          Parse.User.logOut();
           checkLogin();
      });
      
      
      /*user login*/
      $("#login").submit(function(event){
        event.preventDefault();
          
          var name= $("#login-name").val();
          var password=$("#login-password").val();
          
          Parse.User.logIn(name, password, {success:function(){
                 checkLogin();
          }, error:function(user,error){
                console.log("login failed"+error.message);  
          }
        });
      });
      
      /*User signUp*/
      $("#signup").submit(function(event){
        event.preventDefault();
          
          var name= $("#signup-name").val();
          var password=$("#signup-password").val();
          
          var user= new Parse.User();
          user.set("username", name);
          user.set("password", password);
          
          user.signUp(null, {success: function(){
          
          },error:function(user, error){
                console.log("signup error:"+error.message);
          }
        });
          
          
      });
      
      /*Posting the data*/
      
    
      $("#post-form").submit(function(event){
        event.preventDefault();
      var content=$("#post-content").val();
      
      var newPost= new Post();
      newPost.set("chat",content);
    
      newPost.save({success:function(){
      
      },error:function(error){
            console.log("Error"+error.message);
      }
                   });
      });
      
      
      /*Retrieve the data*/
      
      function getPost(){
        var query= new Parse.Query(Post);
          query.find({success:function(result){
              var output="";
            for (var i in result){
                var chat =result[i].get("chat");
                output+="<li>";
                output+="<h4>"+chat+"</h4>";
                output+="</li>";
                
            }
              $("#list-post").html(output);
          },error:function(error){
              console.log("Error:"+error.message);
          }})
      }
       $("#post-submit").click(function(){
           getPost();
           });