var bLoggedIn = false;

var messagesArray = [];


$(document).ready(function(){
    

      function initUI(){
         
         if(Parse.User.current() == null){
              $('#chatting_area').hide();
              $('#login_area').show();
              return false;
         }
          
         if(Parse.User.current().get("username") == undefined){
             $('#chatting_area').hide();
             return;
         }
         else{
             $('#login_area').hide();
             $('#chatting_area').show();
             
         }
          
         var mySelect = $('#user_list');  
          
         
          
          
         var query = new Parse.Query(Parse.User);
            query.notEqualTo("username", Parse.User.current().get("username"));
          
            query.find().then(function(results) {
              
                for (var i = 0; i < results.length; ++i) {
                   var name = results[i].get("username");
                     mySelect.append(
                        $('<option></option>').val(name).html(name)
                    );
                }
                
              }, function(error) {
                
              });

          getMessage();
      }
    
      initUI();
    
      function checkLogin(){
        if (Parse.User.current()){
           
            $("#current-user").html("user: "+Parse.User.current().get("username"));
        }
          else{
            $("#current-user").html("");
          }
      }
       
       
      
      /*User LogOut*/
       $("#logout").click(function(event){
                    
          Parse.User.logOut();
          checkLogin();
          initUI();
      });
      
      
      /*user login*/
      $("#login").click(function(event){
       
          
          var name= $("#login-name").val();
          var password=$("#login-password").val();
          
          Parse.User.logIn(name, password, {success:function(){
                 checkLogin();
              
              UpdateUI();
              
              getMessage();
              
          }, error:function(user,error){
                console.log("login failed"+error.message);  
          }
        });
      });
      
    
      function UpdateUI(){
          if(  Parse.User.current().get("username") != undefined)
          {
             $('#login_area').hide();    
             $('#chatting_area').show();
          
              var mySelect = $('#user_list');  
          
         var query = new Parse.Query(Parse.User);
            query.notEqualTo("username", Parse.User.current().get("username"));
          
            query.find().then(function(results) {
              
                mySelect.find('option').remove();
                
                for (var i = 0; i < results.length; ++i) {
                   var name = results[i].get("username");
                     mySelect.append(
                        $('<option></option>').val(name).html(name)
                    );
                }
                
              }, function(error) {
                
              });
          
          
          
          }
      }
    
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
          
          
        var receiver = $('#user_list').val();
        var messageText = $('#post-content').val();
        var Message = Parse.Object.extend("message");
        var newMessage = new Message();

        newMessage.set("sender", Parse.User.current().get("username"));
        newMessage.set("receiver", receiver);
        newMessage.set("message", messageText);

        newMessage.save(null, {
          success: function(newMessage) {
            // Execute any logic that should take place after the object is saved.
            alert('succeed');
          },
          error: function(newMessage, error) {
            // Execute any logic that should take place if the save fails.
            // error is a Parse.Error with an error code and message.
            alert('failed');
          }
        });  
          
      });
      
    
      // display message
    
      function displayMessage(number, message, createdTime){
          
        
          
          $('#message_body').append('<tr><td>' + number + '</td><td>' + message + '</td><td>' + createdTime + '</td></tr>');
          
           
          
      }
      
      //load message
      function getMessage(){
          
           var sender = $('#user_list').val();
           if(sender == null){
               
               setTimeout(getMessage, 1000);
               return false;
           } 
            
           console.log("message is loading");
          
          //in this case is the current user
          if( Parse.User.current() != null){
              
              var receiver = Parse.User.current().get("username");
          
           var query =  new Parse.Query("message");
            query.equalTo("receiver", receiver);
            query.equalTo("sender", sender);
          
            query.find().then(function(results) {
              
                if(messagesArray.length == 0){
                    
                   for (var i = 0; i < results.length; ++i) {
                     
                      messagesArray.push(results[i]);
                      var createdTime = results[i].get('createdAt');
                      
                      var localDateTime = createdTime.toLocaleDateString();
                       localDateTime =  localDateTime +  ' ' + createdTime.toLocaleTimeString();
                      displayMessage(i+1, results[i].get('message'), localDateTime); 
                       
                    
                   }
                }
                else if(results.length > messagesArray.length){
                    
                    for (var i = messagesArray.length; results.length; i++){
                        
                        messagesArray.push(results[i]);
                        var createdTime = results[i].get('createdAt');
                        
                         var localDateTime = createdTime.toLocaleDateString();
                        localDateTime += ' ' + createdTime.toLocaleTimeString();
                        
                        displayMessage(i+1, results[i].get('message'), localDateTime); 
                    }                
                }
                
              }, function(error) {
                
              });
              
          } 
          
          
            
          setTimeout(getMessage, 1000);
          
      }
     
    
      $('#user_list').change(function(){
          
          messagesArray = [];
          $('#message_body').find('tr').remove();
          //$('li').remove();
          
      })
    
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
    
    
})


 