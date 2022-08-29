import { GetRandomInteger, Register } from "./CommonCode";

describe("add balance flow", () => {
  it("should register and go to my profile page and add balance", () => {
    Register();

    cy.contains("My Profile").click();

    const randomNumber = GetRandomInteger(0, 1000);

    cy.get("#currentBalance")
      .invoke("val")
      .then((currentBalance) => {
        const currentBalanceNumber = Number(currentBalance);
        cy.get("#addBalance").type(randomNumber.toString());
        cy.get("#addBalanceButton").click();

        cy.get("#currentBalance")
          .invoke("val")
          .should((newBalance) => {
            expect(newBalance).to.equal(
              (currentBalanceNumber + randomNumber).toString()
            );
          });
      });
  });
});
