export const GetRandomInteger = (min: number, max: number): number => {
  return Math.floor(Math.random() * (max - min + 1)) + min;
};

export const Login = () => {
  cy.get("#email").type("test@test.com");
  cy.get("#password").type("Test@test.com1");
  cy.wait(600);
  cy.contains("Login").click();
};

export const NavigateTo = (url: string) => {
  cy.get("a").contains(url).click();
  cy.url().should("eq", `http://localhost:3000/${url.toLowerCase()}`);
};

export const Register = () => {
  cy.visit("localhost:3000/");
  cy.url().should("eq", "http://localhost:3000/");
  cy.contains("Register").click();
  cy.wait(300);

  const randomNumber = GetRandomInteger(0, 1000);
  cy.get("#email").type(`test${randomNumber}@email.com`);
  cy.get("#password").type(`Test${randomNumber}password@`);
  cy.get("#fullName").type("Full name");
  cy.wait(1000);

  cy.contains("Submit").click();
  cy.wait(300);
  cy.url().should("eq", "http://localhost:3000/auctions");
};
