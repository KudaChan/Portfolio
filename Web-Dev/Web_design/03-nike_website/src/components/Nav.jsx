import { useState } from 'react';
import { headerLogo } from '../assets/images';
import { hamburger } from '../assets/icons';
import { navLinks } from '../constants';

const Nav = () => {
    const [isOpen, setIsOpen] = useState(false);

    const handleclick = () => {
        setIsOpen(!isOpen);
    };

    const handlelinkclick = () => {
        setIsOpen(false);
    };

    return (
        <header className=' padding-x py-3 fixed z-[1000] w-full bg-white'>
            <nav className='flex justify-between items-center max-container'>
                <a href="/">
                    <img
                        src={headerLogo}
                        alt='logo'
                        width={130}
                        height={29}
                    />
                </a>
                <ul className='flex-1 flex justify-center items-center gap-16 max-lg:hidden'>
                    {navLinks.map((item, i) => (
                        <li key={i}>
                            <a href={item.href}
                                className='font-montserrat leading-normal text-lg text-slate-gray hover:text-coral-red'
                            >
                                {item.label}
                            </a>
                        </li>
                    ))}
                </ul>
                <div className='hidden max-lg:block cursor-pointer z-50' onClick={handleclick}>
                    <img
                        src={hamburger}
                        alt='hamburger'
                        width={24}
                        height={24}
                    />
                </div>
                {isOpen && (
                    <ul className='absolute top-[3.75rem] right-5 bg-slate-200 p-5 shadow-xl rounded-lg flex flex-col gap-5 max-lg:block'>
                        {navLinks.map((item, i) => (
                            <li key={i}>
                                <a href={item.href}
                                    className='font-montserrat leading-normal text-lg text-slate-gray hover:text-coral-red'
                                    onClick={handlelinkclick}
                                >
                                    {item.label}
                                </a>
                            </li>
                        ))}
                    </ul>
                )}
            </nav>
        </header>
    )
}

export default Nav