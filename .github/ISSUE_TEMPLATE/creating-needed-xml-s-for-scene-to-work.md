---
name: Creating Needed XML's for scene to work
about: 'CreatingXML''s  for scene '
title: Creating Needed XML's for scene to work
labels: 'Feature - Branch, Priority: Highest, Protocol preparations'
assignees: ''

---

## Creating Needed XML's for scene to work
Scenes use multiple XML's to work. We will discuss all of them and look at how to connect them to game properly.  

**Actions XML**  
Probably the most important one. The actions XML is used to define steps in the protocol. To create one you can duplicate an excisting XML file or create a new one. Make sure that the name of the actions XML always starts with 'Actions_'. That way people can find the XMl easier when searching in the project. When you have created this XML please fill in every step aswell by Adding the issues in the column ' Protocol Steps' on the gitboard as steps. Use the number as index. Add the step name as a comment in the XML so for example below.

![image](https://user-images.githubusercontent.com/22809437/54311054-1ccb9c80-45d4-11e9-9aca-216bbb436724.png)

> comment="Open bladder spool bag package"   
> index="3"  

Also add the most used values to the step so: 

>   <action  
>     comment="3. Open bladder spool bag package"  
>     index="3"  
>     points=""  
>     description=""  
>     fullDescription=""  
>     type=""  
>     extra=""  
>   ></action>  
>   

If an issue in column has more number so (9-19). This means it is a sequence. For these steps please add: 

> <action  
>   comment=""  
>   index="9"  
>   points=""  
>   description=""  
>   fullDescription=""  
>   type="sequenceStep"  
>   extra=""  
>   value=""  
> ></action>  

_!!!Please do this for every step in colomn ' in progress' (which belong to milestone protocol)!!!_    

**Connecting XML to scene**  
To connect the XML to the scene you have to fill in the name of the Actions XML in the GameLogic prefab which should be in the scene. Select the GameLogic prefab. Then scroll to the component called "Action Manager (script). Then enter the XML name in the input field.  
![image](https://user-images.githubusercontent.com/22809437/54315603-d596d900-45de-11e9-891b-eb69a70d285c.png)

***
**Combinations XML**  
The combinations XMl is used to check combination and create new objects after combination. This is mostly used for the USeOn and Combine action. To create one you can duplicate an excisting XML file or create a new one. Make sure that the name of the combinations XML always starts with 'Combinations_'. That way people can find the XMl easier when searching in the project.  
  
The combinations XML takes combination looking like this:  
`<combination leftInput="CWB_inner_open" rightInput="catheter_bag_twisted" leftResult="" rightResult=""></combination>`  
  
It always takes left input and right input and gives two results. Not every value needs to be filled for it to work. For instance taking off a cap from a needle could be:  

`<combination leftInput="**SyringeWithCap**" rightInput="" leftResult="**SyringeNoCap**" rightResult="**Cap**"></combination>`  

**Connecting XML to scene**   
To connect the XML to the scene you have to fill in the name of the Combinations XML in the GameLogic prefab which should be in the scene. Select the GameLogic prefab. Then scroll to the component called "Combination Manager (script). Then enter the XML name in the input field.  
![image](https://user-images.githubusercontent.com/22809437/54315653-f9f2b580-45de-11e9-89a5-7aeab2a084b6.png)


***

**Animationsequence XML**  
The Animationsequence XMl is used to show multiple option to the player during Animation Sequences. Player has to pick the correct option to continue. To create one you can duplicate an excisting XML file or create a new one. Make sure that the name of the Animationsequence XML always starts with 'AnimSeq_'. That way people can find the XMl easier when searching in the project.  

When Creating a Question in a AnimSeq XML it will look like this:  
![image](https://user-images.githubusercontent.com/22809437/54314999-666cb500-45dd-11e9-907e-50e40676aa7c.png)  
1. Make sure you use `<step> </step>` to indicate a question.  
2. Then Create a option by using `<option> </option>`  
3. Within the `<option> </option>` Create a `text=""` value. This value contains the text which is shown to the player.  
4. Within the `<option> </option>` also add a `animation=""` value. Fill in the value with a name when it is a the correct option. If it is not the correct option. Leave the value empty.  

_!!!The value you enter in the `animation=""` is important and needs to be the same as the value for an animation sequence step in the Actions XML!!!_   

***
**Quiz XML**  
The Quiz XMl is used to show players theory questions about the protocol. Player has to pick the correct anwser to continue. To create one you can duplicate an excisting XML file or create a new one. Make sure that the name of the quiz XML always starts with 'Quiz_'. That way people can find the XMl easier when searching in the project.  

When Creating a Question in a Quiz XML it will look like this:  
![image](https://user-images.githubusercontent.com/22809437/54316009-ef84eb80-45df-11e9-9eec-a714db9aff7a.png)  
1. Make sure you use `<step> </step>` to indicate a block. This block will contain multiple questions. Game will randomly pick a question to display to the player. The second `<step> </step>` will show questions within that block the second time player is asked a question.  
2. Then create a question by using `<question> </question>`. 
3. Within the `<question> </question>` Create a `text=""` value. This value contains the question which is shown to the player.  
4. Within the `<question> </question>` also add `answer=""`. The value takes an integer. This integer number will determin what is the correct answer.  
5. Within the `<question> </question>` also add `points=""` value. Fill in a integer value. This is the amount of points a player gets for answering the question correctly.   
6. Now create an answer the player can choose by using `<answer> </anwser>`. Use the `text=""` value to display the answer and use the `description=""` value to show the player the result when they have picked a question (right or wrong)   

_!!!Make sure that the answer=""` value in `<question> </question>` is the correct number. So if you fill in '2'. Then the second `<answer> </anwser>` Should be the correct awnser.!!!_  

**Connecting XML to scene**  
To connect the XML to the scene you have to fill in the name of the Quiz XML in the PlayerSpawn prefab which should be in the scene. Select the PlayerSpawn prefab. Then scroll to the component called "Player Spawn (script). Then enter the XML name in the input field called 'Quiz Name'  
![image](https://user-images.githubusercontent.com/22809437/54316709-b8afd500-45e1-11e9-880d-7aed9073b102.png)  

## Using/placing prefabs/XML (actions/combinations/Quiz) in scene which already exist
When starting a new scene you should always look for prefabs of object which allready have been made and place them in scene. This also counts for XML actions/combinations etc. Try to add as much existing stuff as possible so other people do not have to.
