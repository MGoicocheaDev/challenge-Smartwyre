# Smartwyre Developer Test Instructions

In the 'RebateService.cs' file you will find a method for calculating a rebate. At a high level the steps for calculating a rebate are:

 1. Lookup the rebate that the request is being made against.
 2. Lookup the product that the request is being made against.
 2. Check that the rebate and request are valid to calculate the incentive type rebate.
 3. Store the rebate calculation.

What we'd like you to do is refactor the code with the following things in mind:

 - Adherence to SOLID principles
 - Testability
 - Readability
 - Currently there are 3 known incentive types. In the future the business will want to add many more incentive types. Your solution should make it easy for developers to add new incentive types in the future.

We’d also like you to 
 - Add some unit tests to the Smartwyre.DeveloperTest.Tests project to show how you would test the code that you’ve produced 
 - Run the RebateService from the Smartwyre.DeveloperTest.Runner console application accepting inputs

The only specific 'rules' are:

- The solution should build
- The tests should all pass

You are free to use any frameworks/NuGet packages that you see fit. You should plan to spend around 1 hour completing the exercise.

---

**Summary of Refactoring with Applied SOLID Principles:**

1. **Single Responsibility Principle (SRP):**
   - An interface (`IRebateCalculator`) and concrete classes for calculation strategies were created. Each class is responsible for a single task: calculating the rebate based on the incentive type

2. **Open/Closed Principle (OCP):**
   - The code structure allows for the addition of new calculation strategies without modifying existing code. This is achieved through the introduction of the IRebateCalculator interface and its implementations.

3. **Liskov Substitution Principle (LSP):**
   - Since there is no direct inheritance in the calculation strategies, this principle is not directly applicable. However, the `IRebateCalculator` interface ensures that all implementations are interchangeable.

4. **Interface Segregation Principle (ISP):**
   - The `IRebateCalculator` interface is specifically designed for each implementation to provide only the necessary methods for its functionality, avoiding the implementation of unnecessary methods.

5. **Dependency Inversion Principle (DIP):**
   - `RebateService` depends on abstractions (`IProductDataStore`, `IRebateDataStore`, and `IRebateCalculator`) rather than concrete implementations. This facilitates dependency injection and enhances code flexibility.

**Additional Outcomes:**
   - The code now follows a dependency injection approach to improve testability and facilitate future extensions.
   - A custom exception (`RebateException`) was introduced to handle specific errors related to rebate calculation.
   - A dictionary was used to handle strategies, improving code readability and maintainability.

