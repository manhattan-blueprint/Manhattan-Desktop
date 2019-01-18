# Style Guide 

_Updated 18-01-2019_

- Use 4 spaces as indentation

- Everything must use `camelCase`, no underscores

- Functions and variables must be declared with an access modifier `public`, `private` etc.

  - Default to private unless it needs to be accessed elsewhere

- Public functions must have a leading capital

  ```csharp
  public void MyExampleFunction(int myParameter) {
      ...
  }
  
  private void myOtherExample(string myParameter) {
      ...
  }
  ```

- There must be a space between the parens and braces

- Do not set values in the editor unless necessary - if this is needed keep them private and use:

  ```csharp
  [SerializeField] private GameObject myObject;
  ```

- Bracing is always on the same line

  ```csharp
  if (true == true) {
      
  } else if (false == false) {
      
  } else {
      
  }
  
  for (int i = 0; i < 3; i++) {
      
  }
  ```

- Explicitly type each variable, avoid using `var myVariable` and instead write `String myVariable`

- Namespace the file as per the file structure 

  ```
  | - 📁 Controller
  |   | - 📁 Subfolder
  |   	 | - 📝 MyFile.cs
  ...    	 
  ```

  ```csharp
  namespace Controller.Subfolder {
      public class MyFile {
          ...
      }
  }
  ```

- A comment should be at the top of each file, explaining its purpose and how it is implemented 

  ```csharp
  // MyClass provides an interface to take food and eat it
  namespace Controller.Subfolder {
      public class MyClass {
          ...
      }
  }
  ```

- Comments should be short and concise and explain **WHY** not **HOW**
