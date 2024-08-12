import PropTypes from 'prop-types';

const Button = ({ label, iconUrl, backgroundColor, borderColor, textColor, fullWidth }) => {
    return (
        <button aria-label={label}
            className={`flex justify-center items-center gap-2 px-7 py-4 border font-montserrat text-lg leading-none rounded-full
            ${backgroundColor ? `${backgroundColor} ${borderColor} ${textColor} hover:bg-coral-red hover:text-white hover:border-coral-red` : 'bg-coral-red text-white border-coral-red'
                } ${fullWidth && 'w-full'}`
        }>
            {label}
            {iconUrl && <img
                src={iconUrl}
                alt='arrow'
                className="ml-2 rounded-full w-5 h-5"
            />}
        </button>
    );
};

Button.propTypes = {
    label: PropTypes.string.isRequired,
    iconUrl: PropTypes.string.isRequired,
    backgroundColor: PropTypes.string,
    borderColor: PropTypes.string,
    textColor: PropTypes.string,
    fullWidth: PropTypes.string,
};

export default Button;