import {useState} from "react";
import {PriceFilter} from "../../filters/priceFilter/PriceFilter";
import {YearFilter} from "../../filters/yearFilter/YearFilter";

export const Filter = ({onFilter, filterName}) => {
    const [isOpen, setIsOpen] = useState(false);

    const closeHandler = () => {
        setIsOpen(false);
    }

    return (
        <>
            <button onClick={() => setIsOpen(true)}>{filterName}</button>
            {isOpen && filterName === "Price" && <PriceFilter onFilter={onFilter} onClose={closeHandler}/>}
            {isOpen && filterName === "Year" && <YearFilter onFilter={onFilter} onClose={closeHandler}/>}
        </>
    );
}
