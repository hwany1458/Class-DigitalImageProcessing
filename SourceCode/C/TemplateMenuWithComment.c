// These lines include standard input, output and standard library functions, respectively.
#include <stdio.h>
#include <stdlib.h> // For exit()

// This is the main function where the program execution begins
int main() {
    // Declares an integer variable choice to store the user's menu selection
    int choice;


    // This do-while loop ensures the menu is displayed and input is taken at least once. 
    // The loop continues as long as choice is not equal to 0 (the exit option)
    do {
        // Display the menu
        // Prints the menu options to the console
        printf("\n--- Menu ---\n");
        printf("1. Option A\n");
        printf("2. Option B\n");
        printf("3. Option C\n");
        printf("0. Exit\n");
        printf("Enter your choice: ");

        // Get user input
        // Reads an integer from the user and stores it in the choice variable.
        // Deprecated function
        // For more detail, https://msdn.microsoft.com/ko-kr/library/w40768et(v=vs.110).aspx
        //scanf("%d", &choice);
        scanf_s("%d", &choice);

        // Process the choice using a switch statement
        // A switch statement is used to execute different code blocks based on the value of choice.
        switch (choice) { // These case labels correspond to the menu options.
        case 1:
            printf("You selected Option A.\n");
            // Add code for Option A
            break;  // Used to exit the switch statement after a case is executed.
        case 2:
            printf("You selected Option B.\n");
            // Add code for Option B
            break;
        case 3:
            printf("You selected Option C.\n");
            // Add code for Option C
            break;
        case 0:
            printf("Exiting program. Goodbye!\n");
            break; // Exits the switch, then the do-while loop
        
        default: // This handles any input that does not match a valid menu option
            printf("Invalid choice. Please try again.\n");
            break;
        }

    } while (choice != 0); // Loop continues until the user chooses to exit (option 4)

    // Indicates successful program execution
    return 0;
}