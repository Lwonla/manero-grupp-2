import ProductGridCard from "../../components/ProductGridCard";
import { apiDomain } from "../../helpers/api-domain";
import useFetch from "../../hooks/useFetch";
import useQuery from "../../hooks/useQuery";
import FilterBurgerMenu from "../../components/Layout/FilterBurgerMenu";
import { useNavigate } from "react-router-dom";

function Products() {
  const navigate = useNavigate();
  const query = useQuery();

  const orderBy = query.get("orderby") ?? "";
  const productCount = useFetch(`${apiDomain.https}/api/products/count?${query.toString()}`);
  const displayAmount = 2;
  const pageButtonAmount = Math.ceil(productCount.data / displayAmount);

  const products = useFetch(
    `${apiDomain.https}/api/products?${query.toString()}&amount=${displayAmount}`
  );

  const handleOrderByChange = (event) => {
    query.set("orderby", event.target.value.toLowerCase());
    navigate(`?${query.toString()}`);
  };

  const updatePageNumber = (pageNumber) => {
    query.set("page", pageNumber);
    navigate(`?${query.toString()}`);
  };

  return (
    <div className="products-page">
      <div className="filter-container">
        <FilterBurgerMenu />
        <select value={orderBy} onChange={handleOrderByChange}>
          <option value="" disabled hidden>
            Order By
          </option>
          <option value="lowestprice">Lowest Price</option>
          <option value="highestprice">Highest Price</option>
        </select>
      </div>
      {products.data?.length == 0 && (
        <div className="no-products-container">
          <h1>We Could Not Find Any Products Based on Your Filtering</h1>
          <p>Please try again with other filtering options.</p>
        </div>
      )}
      <div className="product-card-container">
        {products.data?.map((product) => (
          <ProductGridCard key={product.id} product={product} />
        ))}
      </div>
      <div className="page-buttons">
        {/* TODO: Fix! */}
        {pageButtonAmount > 1 &&
          new Array(5).fill(null).map((_, i) => {
            const currentPage = Number(query.get("page") ?? 1);
            const pageNumber = i + currentPage - 2;

            if (pageNumber > pageButtonAmount) {
              return;
            }
            if (pageNumber <= 0) {
              return;
            }

            return (
              <button
                disabled={pageNumber === currentPage}
                key={i}
                onClick={() => updatePageNumber(pageNumber)}
              >
                {pageNumber}
              </button>
            );
          })}
      </div>
    </div>
  );
}

export default Products;
