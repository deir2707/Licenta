import { useEffect, useState } from "react";
import Api from "../../Api";
import { ApiEndpoints } from "../../ApiEndpoints";
import { PageLayout } from "../../components/PageLayout";
import { useApiError } from "../../hooks/useApiError";
import { StatisticsOutput } from "../../interfaces/ServicesInterfaces";
import { Image } from "../../components/Image";
import { NoImage } from "../../components/NoImage";
import "./StatisticsPage.scss";

export const StatisticsPage = () => {
  const { handleApiError } = useApiError();
  const [statistics, setStatistics] = useState<StatisticsOutput>();

  useEffect(() => {
    Api.get<StatisticsOutput>(ApiEndpoints.get_statistics)
      .then(({ data }) => {
        setStatistics(data);
      })
      .catch((error) => {
        handleApiError(error);
      });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <PageLayout isLoading={!statistics}>
      {statistics?.mostActiveSeller?.fullName ? (
        <div id="statistics-page">
          <div className="by-user-container">
            <div>
              <div>
                <strong>Most expensive item sold by me</strong>
              </div>
              <div className="stat sold-by-user">
                {statistics?.mostExpensiveItemSoldByCurrentUser?.title ? (
                  <>
                    <div>
                      Title:{" "}
                      {statistics?.mostExpensiveItemSoldByCurrentUser?.title}
                    </div>
                    <div>
                      Price:{" "}
                      {
                        statistics?.mostExpensiveItemSoldByCurrentUser
                          ?.currentPrice
                      }
                    </div>
                    <div className="thumbnail">
                      {statistics?.mostExpensiveItemSoldByCurrentUser
                        ?.images?.[0] ? (
                        <Image
                          src={
                            statistics?.mostExpensiveItemSoldByCurrentUser
                              ?.images?.[0]
                          }
                          alt={
                            statistics?.mostExpensiveItemSoldByCurrentUser
                              ?.title
                          }
                        />
                      ) : (
                        <NoImage />
                      )}
                    </div>
                  </>
                ) : (
                  <>You haven't sold any items yet!</>
                )}
              </div>
            </div>
            <div>
              <div>
                <strong>Most expensive item bought by me</strong>
              </div>
              <div className="stat bought-by-user">
                {statistics?.mostExpensiveItemBoughtByCurrentUser?.title ? (
                  <>
                    <div>
                      Title:{" "}
                      {statistics?.mostExpensiveItemBoughtByCurrentUser?.title}
                    </div>
                    <div>
                      Price:{" "}
                      {
                        statistics?.mostExpensiveItemBoughtByCurrentUser
                          ?.currentPrice
                      }
                    </div>
                    <div className="thumbnail">
                      {statistics?.mostExpensiveItemBoughtByCurrentUser
                        ?.images?.[0] ? (
                        <Image
                          src={
                            statistics?.mostExpensiveItemBoughtByCurrentUser
                              ?.images?.[0]
                          }
                          alt={
                            statistics?.mostExpensiveItemBoughtByCurrentUser
                              ?.title
                          }
                        />
                      ) : (
                        <NoImage />
                      )}
                    </div>
                  </>
                ) : (
                  <>You haven't bought any items yet!</>
                )}
              </div>
            </div>
          </div>
          <div className="app-wide-container">
            <div>
              <div>
                <strong>Most active seller</strong>
              </div>
              <div className="stat most-active-seller">
                {statistics?.mostActiveSeller.fullName ? (
                  <>
                    <div>
                      Full Name: {statistics?.mostActiveSeller.fullName}
                    </div>
                    <div>
                      Sold Items: {statistics?.mostActiveSeller.auctions}
                    </div>
                    <div>
                      Money earned: {statistics?.mostActiveSeller.money}
                    </div>
                  </>
                ) : (
                  <>There were no items sold!</>
                )}
              </div>
            </div>
            <div>
              <div>
                <strong>Most active buyer</strong>
              </div>
              <div className="stat most-active-buyer">
                {statistics?.mostActiveBuyer.fullName ? (
                  <>
                    <div>Full Name: {statistics?.mostActiveBuyer.fullName}</div>
                    <div>
                      Sold Items: {statistics?.mostActiveBuyer.auctions}
                    </div>
                    <div>Money spent: {statistics?.mostActiveBuyer.money}</div>
                  </>
                ) : (
                  <>There were no items bought!</>
                )}
              </div>
            </div>
          </div>
        </div>
      ) : (
        <div>There are no auctions finished yet!</div>
      )}
    </PageLayout>
  );
};
