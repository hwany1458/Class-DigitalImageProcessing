#include <stdio.h>
#include <stdlib.h> // For exit()

int main() {
    int choice;

    do {
        // Display the menu
        printf("\n--- Menu ---\n");
        printf("1. Option A\n");
        printf("2. Option B\n");
        printf("3. Option C\n");
        printf("0. Exit\n");
        printf("Enter your choice: ");

        // Get user input
        //scanf("%d", &choice);
        scanf_s("%d", &choice);

        // Process the choice using a switch statement
        switch (choice) {
        case 1:
            printf("You selected Option A.\n");
            // Add code for Option A
            break;
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
        default:
            printf("Invalid choice. Please try again.\n");
            break;
        }

    } while (choice != 0); // Loop continues until the user chooses to exit (option 0)

    return 0;
}